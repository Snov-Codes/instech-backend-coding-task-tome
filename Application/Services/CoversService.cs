using Application.Core;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CoversService : ICoversService
    {
        private readonly ICoversRepository _coversRepository;
        private readonly IAuditsRepository _auditsRepository;

        public CoversService(ICoversRepository coversRepository, IAuditsRepository auditsRepository)
        {
            _coversRepository = coversRepository;
            _auditsRepository = auditsRepository;
        }

        public async Task<Result<Cover>> CreateCoverAsync(Cover cover)
        {
            if(!AreDatesWithinAYear(cover.StartDate, cover.EndDate))
                return Result<Cover>.Failure("Start and End dates of the Cover must be within a one year time span");
            if (cover.StartDate < DateOnly.FromDateTime(DateTime.Now))
                return Result<Cover>.Failure("Cover start date cannot be in the past!");

            cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coversRepository.AddItemAsync(cover);
            await _auditsRepository.AuditCoverAsync(cover.Id, "POST");

            return Result<Cover>.Success();
        }

        public async Task<Result<Cover>> DeleteCoverByIdAsync(string id)
        {
            try
            {
                await _coversRepository.DeleteItemAsync(id);
            }
            catch(Exception ex)
            {
                return Result<Cover>.Failure(ex.Message);
            }

            await _auditsRepository.AuditCoverAsync(id, "DELETE");

            return Result<Cover>.Success();
        }

        public async Task<Result<Cover>> GetCoverByIdAsync(string id)
        {
            var result = await _coversRepository.GetItemAsync(id);

            if (result == null)
                return Result<Cover>.NotFound();

            return Result<Cover>.Success(result);
        }

        public async Task<Result<IEnumerable<Cover>>> GetAllCoversAsync()
        {
            return Result<IEnumerable<Cover>>.Success(await _coversRepository.GetItemsAsync());
        }

        public decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            decimal baseRate = 1250m;
            decimal multiplier = GetMultiplierForCoverType(coverType);
            decimal premiumPerDay = baseRate * multiplier;
            int insuranceLength = endDate.DayNumber - startDate.DayNumber + 1; // Found off by one bug here
            decimal totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                decimal discount = CalculateDiscount(i, coverType);
                totalPremium += ApplyDiscount(premiumPerDay, discount);
            }

            return totalPremium;
        }

        private decimal GetMultiplierForCoverType(CoverType coverType)
        {
            switch (coverType)
            {
                case CoverType.Yacht:
                    return 1.1m;
                case CoverType.PassengerShip:
                    return 1.2m;
                case CoverType.Tanker:
                    return 1.5m;
                default:
                    return 1.3m;
            }
        }

        private decimal CalculateDiscount(int day, CoverType coverType)
        {
            if (day < 30)
            {
                return 0m;
            }
            else if (day < 180)
            {
                return (coverType == CoverType.Yacht) ? 0.05m : 0.02m;
            }
            else
            {
                return (coverType == CoverType.Yacht) ? 0.08m : 0.03m;
            }
        }

        private decimal ApplyDiscount(decimal premiumPerDay, decimal discount)
        {
            return premiumPerDay - (premiumPerDay * discount);
        }

        private bool AreDatesWithinAYear(DateOnly startDate, DateOnly endDate)
        {
            DateOnly oneYearLater = startDate.AddYears(1);
            return endDate <= oneYearLater && endDate >= startDate;
        }
    }
}
