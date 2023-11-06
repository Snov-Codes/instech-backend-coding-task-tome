﻿using Application.Interfaces.Services;
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

        private decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
        {
            var multiplier = 1.3m;
            if (coverType == CoverType.Yacht)
            {
                multiplier = 1.1m;
            }

            if (coverType == CoverType.PassengerShip)
            {
                multiplier = 1.2m;
            }

            if (coverType == CoverType.Tanker)
            {
                multiplier = 1.5m;
            }

            var premiumPerDay = 1250 * multiplier;
            var insuranceLength = endDate.DayNumber - startDate.DayNumber;
            var totalPremium = 0m;

            for (var i = 0; i < insuranceLength; i++)
            {
                if (i < 30) totalPremium += premiumPerDay;
                if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
                else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
                if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
                else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
            }

            return totalPremium;
        }
    }
}
