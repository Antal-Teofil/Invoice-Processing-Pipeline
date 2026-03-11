using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceProcessingPipeline.Functions.Activities;

// to be refactored
public sealed class RequestDocumentAccessibilityActivity(
    ILogger<RequestDocumentAccessibilityActivity> logger,
    BlobServiceClient blobServiceClient,
    IOptions<DocumentStorageOptions> options)
{
    [Function(nameof(RequestDocumentAccessibilityActivity))]
    public async Task<ActivityResult<DocumentSasUri>> RunAsync(
        [ActivityTrigger] DocumentIngestionEvent ingestionEvent,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ingestionEvent);
        ArgumentNullException.ThrowIfNull(ingestionEvent.StorageMetadata);
        ArgumentNullException.ThrowIfNull(ingestionEvent.EventMetadata);

        try
        {
            var correlationId = ingestionEvent.CorrelationId;
            var blobUrl = ingestionEvent.StorageMetadata.DocumentUrl;

            logger.LogInformation(
                "Generating document SAS. CorrelationId: {CorrelationId}, DocumentUrl: {DocumentUrl}",
                correlationId,
                blobUrl);

            if (string.IsNullOrWhiteSpace(blobUrl))
            {
                return ActivityResult<DocumentSasUri>.Failure(
                    "DocumentStorageMetadata.DocumentUrl is missing.");
            }

            if (!Uri.TryCreate(blobUrl, UriKind.Absolute, out var blobUriValue))
            {
                return ActivityResult<DocumentSasUri>.Failure(
                    "DocumentStorageMetadata.DocumentUrl is not a valid absolute URI.");
            }

            var blobUri = new BlobUriBuilder(blobUriValue);

            if (string.IsNullOrWhiteSpace(blobUri.BlobContainerName))
            {
                return ActivityResult<DocumentSasUri>.Failure(
                    "Blob container name could not be resolved from DocumentUrl.");
            }

            if (string.IsNullOrWhiteSpace(blobUri.BlobName))
            {
                return ActivityResult<DocumentSasUri>.Failure(
                    "Blob name could not be resolved from DocumentUrl.");
            }

            var configuredServiceUri = new Uri(options.Value.ServiceUri);
            var blobAccountUri = new Uri($"{blobUri.Scheme}://{blobUri.Host}");

            if (Uri.Compare(
                    configuredServiceUri,
                    blobAccountUri,
                    UriComponents.SchemeAndServer,
                    UriFormat.Unescaped,
                    StringComparison.OrdinalIgnoreCase) != 0)
            {
                return ActivityResult<DocumentSasUri>.Failure(
                    "DocumentUrl does not belong to the configured blob storage account.");
            }

            var ttlMinutes = options.Value.SasTtlMinutes > 0
                ? options.Value.SasTtlMinutes
                : 10;

            var startsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
            var sasExpiresOn = DateTimeOffset.UtcNow.AddMinutes(ttlMinutes);
            var keyExpiresOn = DateTimeOffset.UtcNow.AddHours(1);

            Response<UserDelegationKey> delegationKeyResponse =
                await blobServiceClient.GetUserDelegationKeyAsync(
                    startsOn,
                    keyExpiresOn,
                    cancellationToken);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobUri.BlobContainerName,
                BlobName = blobUri.BlobName,
                Resource = "b",
                StartsOn = startsOn,
                ExpiresOn = sasExpiresOn,
                Protocol = SasProtocol.Https,
                CorrelationId = correlationId
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobClient = blobServiceClient
                .GetBlobContainerClient(blobUri.BlobContainerName)
                .GetBlobClient(blobUri.BlobName);

            var sasUri = blobClient.GenerateUserDelegationSasUri(
                sasBuilder,
                delegationKeyResponse.Value);

            logger.LogInformation(
                "Document SAS generated successfully. CorrelationId: {CorrelationId}, ExpiresOnUtc: {ExpiresOnUtc}",
                correlationId,
                sasExpiresOn);

            return ActivityResult<DocumentSasUri>.Success(new DocumentSasUri
            {
                SasUri = sasUri
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to generate SAS for CorrelationId: {CorrelationId}", ingestionEvent.CorrelationId);

            return ActivityResult<DocumentSasUri>.Failure(
                $"Failed to generate SAS URI: {ex.Message}");
        }
    }
}