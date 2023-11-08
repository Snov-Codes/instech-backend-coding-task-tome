﻿using Application.Interfaces.Services;
using Domain.Entities;
using Persistance.Interfaces;

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

        public async Task CreateClaimAsync(Claim claim)
        {
            var cover = await _coversRepository.GetItemAsync(claim.CoverId);
            if (cover is not null)
            {
                await _claimsRepository.AddItemAsync(claim);
                await _auditsRepository.AuditClaimAsync(claim.Id, "POST");
            }
        }

        public async Task DeleteClaimByIdAsync(string id)
        {
            await _claimsRepository.DeleteItemAsync(id);
            await _auditsRepository.AuditClaimAsync(id, "DELETE");
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
