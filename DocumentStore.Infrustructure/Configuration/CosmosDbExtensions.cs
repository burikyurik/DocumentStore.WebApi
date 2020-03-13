using System;
using DocumentStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentStore.Infrastructure.Configuration
{
    public static class CosmosDbExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, string endpoint, string authKey, string dataBaseName)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentNullException("Please specify a valid endpoint in the appSettings.json file.");
            }
            if (string.IsNullOrEmpty(authKey))
            {
                throw new ArgumentException("Please specify a valid AuthorizationKey in the appSettings.json file.");
            }

            services.AddDbContext<DocumentContext>(builder =>
                builder.UseCosmos(endpoint, authKey, dataBaseName));
            
            services.AddScoped<IDomainRepository, CosmosDbDomainRepository>();
            return services;
        }
    }
}