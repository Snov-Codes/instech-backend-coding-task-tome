using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Persistance.Interfaces;

namespace Persistance.Repositories
{
    public class ClaimsRepository : CosmosDbRepository<Claim>, IClaimsRepository
    {
        public override string ContainerName { get; } = "Claim";

        public override string GenerateId(Claim entity) => Guid.NewGuid().ToString();

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public ClaimsRepository(ICosmosDbContainerFactory factory) : base(factory) { }
    }
}
