using Application.Interfaces.Repositories;
using Persistance.AppSettings;
using Persistance.Repositories;

namespace Claims.Extensions.Config
{
    public static class DatabaseConfig
    {
        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind database-related bindings
            CosmosDbConfig cosmosDbConfig = configuration.GetSection("CosmosDB").Get<CosmosDbConfig>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);

            services.AddScoped<IClaimsRepository, ClaimsRepository>();
            services.AddScoped<ICoversRepository, CoversRepository>();
        }
    }
}
