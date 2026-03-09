using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations;

public static class CosmosServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosClient(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<CosmosClientOptions>()
            .BindConfiguration(CosmosClientOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.ResourceEndpoint, UriKind.Absolute, out _),
                "ResourceEndpoint must be a valid absolute URI.");

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosClientOptions>>().Value;
            var credential = sp.GetRequiredService<TokenCredential>();

            return new CosmosClient(
                accountEndpoint: options.ResourceEndpoint,
                tokenCredential: credential);
        });

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosClientOptions>>().Value;
            var client = sp.GetRequiredService<CosmosClient>();

            return client.GetDatabase(options.DatabaseName);
        });

        return services;
    }

    public sealed record CosmosClientOptions
    {
        public const string SectionName = "COSMOS_CLIENT_RESOURCE_ACCESS";

        [Url]
        public required string ResourceEndpoint { get; init; }
        public required string DatabaseName { get; init; }
    }
}

