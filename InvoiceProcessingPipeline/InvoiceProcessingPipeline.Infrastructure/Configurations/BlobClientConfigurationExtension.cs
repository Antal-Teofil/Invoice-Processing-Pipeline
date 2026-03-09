using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations;

public static class BlobServiceCollectionExtensions
{
    public static IServiceCollection AddBlobClient(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<BlobStorageOptions>()
            .BindConfiguration(BlobStorageOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.ServiceUri, UriKind.Absolute, out _),
                "ServiceUri must be a valid absolute URI.");

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<BlobStorageOptions>>().Value;
            var credential = sp.GetRequiredService<TokenCredential>();

            return new BlobServiceClient(
                serviceUri: new Uri(options.ServiceUri),
                credential: credential);
        });

        return services;
    }

    public sealed record BlobStorageOptions
    {
        public const string SectionName = "BLOB_STORAGE";

        [Url]
        public required string ServiceUri { get; init; }
    }
}