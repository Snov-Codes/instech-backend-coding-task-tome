using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICoversService
    {
        Task CreateCoverAsync(Cover cover);
        Task DeleteCoverByIdAsync(string id);
        Task<Cover> GetCoverByIdAsync(string id);
        Task<IEnumerable<Cover>> GetAllCoversAsync();
    }
}
