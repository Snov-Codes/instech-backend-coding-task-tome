using Application.Interfaces.Services;
using Domain.Entities;
using Persistance.Interfaces;

namespace Application.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IClaimsRepository _claimsRepository;

        public ClaimsService(IClaimsRepository claimsRepository)
        {
            _claimsRepository = claimsRepository;
        }

        public async Task CreateClaimAsync(Claim claim)
        {
            await _claimsRepository.AddItemAsync(claim);
        }

        public async Task DeleteClaimByIdAsync(string id)
        {
            await _claimsRepository.DeleteItemAsync(id);
        }

        public async Task<Claim> GetClaimByIdAsync(string id)
        {
            return await _claimsRepository.GetItemAsync(id);
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsAsync()
        {
            return await _claimsRepository.GetItemsAsync();
        }
    }
}
