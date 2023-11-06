﻿using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class CoversRepository : CosmosDbRepository<Cover>, ICoversRepository
    {
        public override string ContainerName { get; } = "Cover";

        public override string GenerateId(Cover entity) => Guid.NewGuid().ToString();

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public CoversRepository(ICosmosDbContainerFactory factory) : base(factory) { }
    }
}
