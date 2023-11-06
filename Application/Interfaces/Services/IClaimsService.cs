using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IClaimsService
    {
        Task CreateClaimAsync(Claim claim);
        Task DeleteClaimByIdAsync(string id);
        Task<Claim> GetClaimByIdAsync(string id);
        Task<IEnumerable<Claim>> GetAllClaimsAsync();
    }
}
