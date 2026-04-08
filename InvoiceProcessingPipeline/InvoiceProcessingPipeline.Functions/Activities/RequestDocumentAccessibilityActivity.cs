using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using static InvoiceProcessingPipeline.Infrastructure.Configurations.BlobServiceCollectionExtensions;

namespace InvoiceProcessingPipeline.Functions.Activities;

/*
    ez a csoda azt tudja, hogy a(z) `DocumentIngestionEvent`-bol kiveszi a szukseges informaciokat, ellenorzi a kapott BLOB URI-t (schema, container, stb.),
    leker egy User Delegation Keyt, majd azt hasznalva keszit egy SAS (Shared Access Signature) URI-t, amit tovabbit a kovetkezo activity function-nek, ami ennek reven hozzaferest kap
    megadott ideig egy adott blobhoz. A kovetkezo activity fogja kivonni a nyers informaciot a dokumentumbol.

    ********************************************************************************************************************
    * a) cancellation token megfelelo kezelese
    * b) configbol allitani a key lejarati idejet
    * c) ne dobjunk nyers kivetelt
    * d) legyen sajat kivetel
    * e) valamiket kene kezdeni a kivetelekkel mert sok van es nagyon csunya.
    * f) talan jo lenne Serilog vagy valami, mert igy kinzas.
    * g) majd kitalalom hogyan kezeljem megfeleloen a correlation id-t, de azt nem ma
*/

[DebuggerDisplay("Activity={GetType().Name,nq}")]
public sealed class RequestDocumentAccessibilityActivity(
    ILogger<RequestDocumentAccessibilityActivity> logger,
    BlobServiceClient blobServiceClient, // blob resource hozzafereshez
    IOptions<BlobStorageOptions> options) // tartalmazza a resource uri-t
{
    [Function(nameof(RequestDocumentAccessibilityActivity))]
    public async Task<ActivityResult<DocumentUserDelegationSasUri>> RunAsync(
        [ActivityTrigger] DocumentIngestionEvent ingestionEvent,
        CancellationToken cancellationToken)
    {
        return await ExecuteAsync(
            ingestionEvent,
            cancellationToken,
            GenerateUserDelegationSasUriAsync);
    }

    private async Task<ActivityResult<DocumentUserDelegationSasUri>> ExecuteAsync(
        DocumentIngestionEvent ingestionEvent,
        CancellationToken cancellationToken,
        Func<DocumentIngestionEvent, CancellationToken, Task<Uri>> callback)
    {
        ArgumentNullException.ThrowIfNull(ingestionEvent);
        ArgumentNullException.ThrowIfNull(ingestionEvent.StorageMetadata);
        ArgumentNullException.ThrowIfNull(ingestionEvent.EventMetadata);

        try
        {
            var sasUri = await callback(ingestionEvent, cancellationToken);

            return ActivityResult<DocumentUserDelegationSasUri>.Success(new DocumentUserDelegationSasUri
            {
                SasUri = sasUri
            });
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(
                "Invalid document SAS request. CorrelationId: {CorrelationId}, Reason: {Reason}",
                ingestionEvent.CorrelationId,
                ex.Message);

            return ActivityResult<DocumentUserDelegationSasUri>.Failure(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(
                "Document SAS generation failed due to invalid state. CorrelationId: {CorrelationId}, Reason: {Reason}",
                ingestionEvent.CorrelationId,
                ex.Message);

            return ActivityResult<DocumentUserDelegationSasUri>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to generate a SAS URI for the document. CorrelationId: {CorrelationId}",
                ingestionEvent.CorrelationId);

            return ActivityResult<DocumentUserDelegationSasUri>.Failure(
                $"Failed to generate the SAS URI for the document: {ex.Message}");
        }
    }

    private async Task<Uri> GenerateUserDelegationSasUriAsync(
        DocumentIngestionEvent ingestionEvent,
        CancellationToken cancellationToken)
    {
        var correlationId = ingestionEvent.CorrelationId;
        var blobUrl = ingestionEvent.StorageMetadata.DocumentUrl;

        logger.LogInformation(
            "Generating a SAS URI for the document. CorrelationId: {CorrelationId}, DocumentUrl: {DocumentUrl}",
            correlationId,
            blobUrl);

        var blobUri = ParseBlobUri(blobUrl);
        ValidateStorageAccount(blobUri);

        var ttlMinutes = options.Value.SasTtlMinutes > 0
            ? options.Value.SasTtlMinutes
            : 10;

        var startsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
        var sasExpiresOn = DateTimeOffset.UtcNow.AddMinutes(ttlMinutes);
        var keyExpiresOn = DateTimeOffset.UtcNow.AddHours(1);

        var delegationKey = await blobServiceClient.GetUserDelegationKeyAsync(
            startsOn,
            keyExpiresOn,
            cancellationToken);

        var sasBuilder = BuildSasBuilder(
            blobUri,
            correlationId,
            startsOn,
            sasExpiresOn);

        var blobClient = blobServiceClient
            .GetBlobContainerClient(blobUri.BlobContainerName)
            .GetBlobClient(blobUri.BlobName);

        var sasUri = blobClient.GenerateUserDelegationSasUri(
            sasBuilder,
            delegationKey.Value);

        logger.LogInformation(
            "The document SAS URI was generated successfully. CorrelationId: {CorrelationId}, ExpiresOnUtc: {ExpiresOnUtc}",
            correlationId,
            sasExpiresOn);

        return sasUri;
    }

    private static BlobUriBuilder ParseBlobUri(string? blobUrl)
    {
        if (string.IsNullOrWhiteSpace(blobUrl))
        {
            throw new ArgumentException(
                "The document URL is missing.",
                nameof(blobUrl));
        }

        if (!Uri.TryCreate(blobUrl, UriKind.Absolute, out var rawUri))
        {
            throw new ArgumentException(
                "The document URL is not a valid absolute URI.",
                nameof(blobUrl));
        }

        var blobUri = new BlobUriBuilder(rawUri, trimBlobNameSlashes: false);

        if (string.IsNullOrWhiteSpace(blobUri.BlobContainerName))
        {
            throw new ArgumentException(
                "The blob container name could not be resolved from the document URL.",
                nameof(blobUrl));
        }

        if (string.IsNullOrWhiteSpace(blobUri.BlobName))
        {
            throw new ArgumentException(
                "The blob name could not be resolved from the document URL.",
                nameof(blobUrl));
        }

        return blobUri;
    }

    private void ValidateStorageAccount(BlobUriBuilder blobUri)
    {
        if (!Uri.TryCreate(options.Value.ServiceUri, UriKind.Absolute, out var configuredServiceUri))
        {
            throw new InvalidOperationException(
                "The configured blob service URI is invalid.");
        }

        var blobAccountUri = new Uri($"{blobUri.Scheme}://{blobUri.Host}");

        if (Uri.Compare(
                configuredServiceUri,
                blobAccountUri,
                UriComponents.SchemeAndServer,
                UriFormat.Unescaped,
                StringComparison.OrdinalIgnoreCase) != 0)
        {
            throw new ArgumentException(
                "The document URL does not belong to the configured blob storage account.");
        }
    }

    private static BlobSasBuilder BuildSasBuilder(
        BlobUriBuilder blobUri,
        string correlationId,
        DateTimeOffset startsOn,
        DateTimeOffset expiresOn)
    {
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobUri.BlobContainerName,
            BlobName = blobUri.BlobName,
            Resource = "b",
            StartsOn = startsOn,
            ExpiresOn = expiresOn,
            Protocol = SasProtocol.Https,
            CorrelationId = correlationId
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        return sasBuilder;
    }
}