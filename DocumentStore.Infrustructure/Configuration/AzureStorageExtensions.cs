using Microsoft.Extensions.DependencyInjection;

namespace DocumentStore.Infrastructure.Configuration
{
    public static class AzureStorageExtensions
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IFileRepository, AzureBlobStorageFileRepository>(provider =>
                new AzureBlobStorageFileRepository(connectionString));

            return services;
        }
    }
}
