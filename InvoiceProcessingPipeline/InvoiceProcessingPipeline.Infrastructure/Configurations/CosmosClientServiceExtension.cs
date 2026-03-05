using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations
{
    public static class CosmosClientServiceExtension
    {
        public static IServiceCollection AddCosmosClientAccessConfiguration(
            this IServiceCollection services)
        {
            services.AddOptionsWithValidateOnStart<CosmosClientAccessConfiguration>()
                .BindConfiguration("COSMOS_CLIENT_RESOURCE_ACCESS")
                .ValidateDataAnnotations();

            services.AddSingleton<DefaultAzureCredential>();

            services.AddSingleton(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<CosmosClientAccessConfiguration>>()
                    .Value;

                var credential = sp.GetRequiredService<DefaultAzureCredential>();

                return new CosmosClient(options.ResourceEndpoint, credential);
            });

            return services;
        }
    }

    public sealed record CosmosClientAccessConfiguration
    {
        [Required]
        public required string ResourceEndpoint { get; init; }

        [Required]
        public required string DatabaseName { get; init; }
    }
}