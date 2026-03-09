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
        services.AddOptionsWithValidateOnStart<CosmosClientSettings>()
            .BindConfiguration(CosmosClientSettings.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.ResourceEndpoint, UriKind.Absolute, out _),
                "ResourceEndpoint must be a valid absolute URI.");

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosClientSettings>>().Value;
            var credential = sp.GetRequiredService<TokenCredential>();

            var cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            return new CosmosClient(
                accountEndpoint: options.ResourceEndpoint,
                tokenCredential: credential,
                clientOptions: cosmosClientOptions);
        });

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosClientSettings>>().Value;
            var client = sp.GetRequiredService<CosmosClient>();

            return client.GetDatabase(options.DatabaseName);
        });

        services.AddKeyedSingleton("invoice-event", (sp, _) =>
        {
            var database = sp.GetRequiredService<Database>();
            return database.GetContainer("invoice-event");
        });

        services.AddKeyedSingleton("invoice-audit", (sp, _) =>
        {
            var database = sp.GetRequiredService<Database>();
            return database.GetContainer("invoice-audit");
        });

        return services;
    }

    public sealed record CosmosClientSettings
    {
        public const string SectionName = "COSMOS_CLIENT_RESOURCE_ACCESS";

        [Url]
        public required string ResourceEndpoint { get; init; }
        public required string DatabaseName { get; init; }
    }
}