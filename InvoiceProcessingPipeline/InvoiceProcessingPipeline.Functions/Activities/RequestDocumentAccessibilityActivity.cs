using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceProcessingPipeline.Functions.Activities;

/// <summary>
/// Provides an activity for generating a Shared Access Signature (SAS) URI that enables secure, time-limited read
/// access to a document stored in Azure Blob Storage, based on an ingestion event.
/// </summary>
/// <remarks>This activity validates the provided document URL and generates a SAS URI with read permissions for
/// the specified blob. The SAS expiration is configurable via options and defaults to 10 minutes if not set. The
/// activity logs the correlation ID for traceability and throws an exception if the document URL is missing or
/// invalid.</remarks>
/// <param name="logger">The logger used to record informational and error messages during the execution of the activity.</param>
/// <param name="blobServiceClient">The Azure BlobServiceClient instance used to interact with Blob Storage and generate user delegation SAS tokens.</param>
/// <param name="options">The configuration options containing settings for document storage, including the SAS time-to-live (TTL) value.</param>
public sealed class RequestDocumentAccessibilityActivity(
    ILogger<RequestDocumentAccessibilityActivity> logger,
    BlobServiceClient blobServiceClient,
    IOptions<DocumentStorageOptions> options)
{


    // Ide fog kelleni egy kurva nagy refaktoralas, de az meg odebb van. majd a user delegation key cache-elve lesz.

    [Function(nameof(RequestDocumentAccessibilityActivity))]
    public async Task<ActivityResult<DocumentSasUri>> RunAsync(
        [ActivityTrigger] DocumentIngestionEvent ingestionEvent) // cancellation tokent majd kezeljuk megfeleloen, csak nem most
    {
        logger.LogInformation("Generating SAS for CorrelationId: {CorrelationId}", ingestionEvent.CorrelationId);

        var blobUrl = ingestionEvent?.StorageMetadata?.DocumentURL;
        if (string.IsNullOrWhiteSpace(blobUrl))
            throw new ArgumentException("DocumentURL is missing.");

        if (!Uri.TryCreate(blobUrl, UriKind.Absolute, out var blobUriValue)) // itt majd az exception be lesz csomagolva egy ActivityResult-be
            throw new ArgumentException("DocumentURL is not a valid absolute URI.");

        var blobUri = new BlobUriBuilder(blobUriValue);

        var ttlMinutes = options.Value.SasTtlMinutes <= 0 ? 10 : options.Value.SasTtlMinutes;

        var startsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
        var sasExpiresOn = DateTimeOffset.UtcNow.AddMinutes(ttlMinutes);
        var keyExpiresOn = DateTimeOffset.UtcNow.AddHours(1);

        // majd cache-be rakom
        var delegationKeyResponse = await blobServiceClient.GetUserDelegationKeyAsync(
            startsOn,
            keyExpiresOn);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobUri.BlobContainerName,
            BlobName = blobUri.BlobName,
            Resource = "b",
            StartsOn = startsOn,
            ExpiresOn = sasExpiresOn
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var blobClient = blobServiceClient
            .GetBlobContainerClient(blobUri.BlobContainerName)
            .GetBlobClient(blobUri.BlobName);

        var sasUri = blobClient.GenerateUserDelegationSasUri(sasBuilder, delegationKeyResponse.Value);

        return ActivityResult<DocumentSasUri>.Success(new DocumentSasUri(sasUri));
    }
}