using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly IAuditsRepository _auditsRepository;
        private readonly ICoversRepository _coversRepository;

        public ClaimsService(IClaimsRepository claimsRepository, IAuditsRepository auditsRepository, ICoversRepository coversRepository)
        {
            _claimsRepository = claimsRepository;
            _auditsRepository = auditsRepository;
            _coversRepository = coversRepository;
        }

        public async Task<Result<Claim>> CreateClaimAsync(Claim claim)
        {
            var cover = await _coversRepository.GetItemAsync(claim.CoverId);
            if (cover is null)
                return Result<Claim>.Failure("Cover is required when creating a Claim!");

            var dateOnly = DateOnly.FromDateTime(claim.Created);

            if (dateOnly < cover.StartDate || dateOnly > cover.EndDate)
                return Result<Claim>.Failure("Claim creation date must be within the Cover start and end dates!");
            if (claim.DamageCost > 100000)
                return Result<Claim>.Failure("The damage cost of the Claim cannot exceed 100.000!");

            await _claimsRepository.AddItemAsync(claim);
            await _auditsRepository.AuditClaimAsync(claim.Id, "POST");

            return Result<Claim>.Success();
        }

        public async Task<Result<Claim>> DeleteClaimByIdAsync(string id)
        {
            try
            {
                await _claimsRepository.DeleteItemAsync(id);
            }
            catch(Exception ex)
            { 
                return Result<Claim>.Failure(ex.Message);
            }

            await _auditsRepository.AuditClaimAsync(id, "DELETE");

            return Result<Claim>.Success();
        }

        public async Task<Result<Claim>> GetClaimByIdAsync(string id)
        {
            var result = await _claimsRepository.GetItemAsync(id);
            if(result is null)
                return Result<Claim>.NotFound();

            return Result<Claim>.Success(result);
        }

        public async Task<Result<IEnumerable<Claim>>> GetAllClaimsAsync()
        {
            return Result<IEnumerable<Claim>>.Success(await _claimsRepository.GetItemsAsync());
        }
    }
}
