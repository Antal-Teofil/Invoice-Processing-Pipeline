using Azure.Core;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations;

public static class QueueServiceCollectionExtensions
{
    public static IServiceCollection AddQueueClient(this IServiceCollection services)
    {
        services.AddOptionsWithValidateOnStart<QueueStorageOptions>()
            .BindConfiguration(QueueStorageOptions.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                options => Uri.TryCreate(options.ServiceUri, UriKind.Absolute, out _),
                "ServiceUri must be a valid absolute URI.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.QueueName),
                "QueueName is required.");

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<QueueStorageOptions>>().Value;
            var credential = sp.GetRequiredService<TokenCredential>();

            var serviceClient = new QueueServiceClient(
                serviceUri: new Uri(options.ServiceUri),
                credential: credential,
                options: new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });

            return serviceClient.GetQueueClient(options.QueueName);
        });

        return services;
    }

    public sealed record QueueStorageOptions
    {
        public const string SectionName = "QUEUE_STORAGE";

        [Url]
        public required string ServiceUri { get; init; }

        [Required]
        public required string QueueName { get; init; }
    }
}