using Domain.Entities;

namespace Persistance.Interfaces
{
    public interface IClaimsRepository : ICosmosDbRepository<Claim>
    {
    }
}
