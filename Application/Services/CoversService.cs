using Application.Interfaces.Services;
using Domain.Entities;
using Persistance.Interfaces;

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

        public async Task CreateCoverAsync(Cover cover)
        {
            cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
            await _coversRepository.AddItemAsync(cover);
            await _auditsRepository.AuditCoverAsync(cover.Id, "POST");
        }

        public async Task DeleteCoverByIdAsync(string id)
        {
            await _coversRepository.DeleteItemAsync(id);
            await _auditsRepository.AuditCoverAsync(id, "DELETE");
        }

        public async Task<Cover> GetCoverByIdAsync(string id)
        {
            return await _coversRepository.GetItemAsync(id);
        }

        public async Task<IEnumerable<Cover>> GetAllCoversAsync()
        {
            return await _coversRepository.GetItemsAsync();
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
    }
}
