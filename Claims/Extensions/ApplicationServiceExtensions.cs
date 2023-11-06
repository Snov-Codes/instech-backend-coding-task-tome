using Application.Interfaces.Services;
using Application.Services;
using Claims.Extensions.Config;
using Microsoft.EntityFrameworkCore;
using Persistance.AppSettings;
using Persistance.DataAccess;
using Persistance.Interfaces;
using Persistance.Repositories;

namespace Claims.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.SetupCosmosDb(config);
            services.AddAuditDb(config);
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<ICoversService, CoversService>();

            return services;
        }

        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerConfig> containers)
        {
            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(endpointUrl, primaryKey);
            CosmosDbContainerFactory cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);
            cosmosDbClientFactory.EnsureDbSetupAsync().Wait();

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }

        public static void AddAuditDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EfDataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAuditsRepository, AuditsRepository>();
        }
    }
}
