using Application.Core;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IClaimsService
    {
        Task<Result<Claim>> CreateClaimAsync(Claim claim);
        Task<Result<Claim>> DeleteClaimByIdAsync(string id);
        Task<Result<Claim>> GetClaimByIdAsync(string id);
        Task<Result<IEnumerable<Claim>>> GetAllClaimsAsync();
    }
}
