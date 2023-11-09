using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IClaimsRepository : ICosmosDbRepository<Claim>
    {
    }
}
