using Application.Core;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICoversService
    {
        Task<Result<Cover>> CreateCoverAsync(Cover cover);
        Task<Result<Cover>> DeleteCoverByIdAsync(string id);
        Task<Result<Cover>> GetCoverByIdAsync(string id);
        Task<Result<IEnumerable<Cover>>> GetAllCoversAsync();
        decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
    }
}
