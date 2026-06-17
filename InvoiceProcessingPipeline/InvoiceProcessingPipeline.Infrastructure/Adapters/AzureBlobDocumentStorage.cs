using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using InvoiceProcessingPipeline.Application.ExportTypes;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters;

public sealed class AzureBlobDocumentStorage(
    ILogger<AzureBlobDocumentStorage> logger,
    [FromKeyedServices(
        BlobServiceCollectionExtensions.DocumentsXmlContainerKey)]
    BlobContainerClient containerClient)
    : IDocumentXmlStore
{
    public async Task<Uri> StoreXmlDocumentSchemeAsync(
        ExportedDocument document,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);

        var blobName = CreateBlobName(document);

        BlobClient blobClient =
            containerClient.GetBlobClient(blobName);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = CreateContentType(document)
            },

            Metadata = CreateMetadata(document),

            /*
             * A GUID-ot tartalmazó blobnév miatt normál esetben
             * nem történhet felülírás. Ez a feltétel ezt szerveroldalon
             * is kikényszeríti.
             */
            Conditions = new BlobRequestConditions
            {
                IfNoneMatch = ETag.All
            }
        };

        try
        {
            await blobClient.UploadAsync(
                BinaryData.FromBytes(document.Content),
                uploadOptions,
                cancellationToken);

            logger.LogInformation(
                "XML document uploaded to Blob Storage. " +
                "FileName: {FileName}, BlobName: {BlobName}, BlobUri: {BlobUri}",
                document.FileName,
                blobName,
                blobClient.Uri);

            return blobClient.Uri;
        }
        catch (RequestFailedException exception)
        {
            logger.LogError(
                exception,
                "Failed to upload XML document to Blob Storage. " +
                "FileName: {FileName}, BlobName: {BlobName}, " +
                "Status: {Status}, ErrorCode: {ErrorCode}",
                document.FileName,
                blobName,
                exception.Status,
                exception.ErrorCode);

            throw;
        }
    }

    private static string CreateBlobName(
        ExportedDocument document)
    {
        var now = DateTimeOffset.UtcNow;

        var safeFileName =
            SanitizeBlobFileName(document.FileName);

        return string.Join(
            '/',
            "invoices",
            now.ToString("yyyy"),
            now.ToString("MM"),
            now.ToString("dd"),
            $"{Guid.NewGuid():N}-{safeFileName}");
    }

    private static string CreateContentType(
        ExportedDocument document)
    {
        if (string.IsNullOrWhiteSpace(document.Encoding))
        {
            return document.ContentType;
        }

        return
            $"{document.ContentType}; " +
            $"charset={document.Encoding.ToLowerInvariant()}";
    }

    private static IDictionary<string, string> CreateMetadata(
        ExportedDocument document)
    {
        var metadata = new Dictionary<string, string>
        {
            ["format"] = document.Format,
            ["originalFileName"] = SanitizeMetadataValue(
                document.FileName)
        };

        if (!string.IsNullOrWhiteSpace(document.Encoding))
        {
            metadata["characterEncoding"] =
                document.Encoding;
        }

        return metadata;
    }

    private static string SanitizeBlobFileName(
        string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "document.xml";
        }

        /*
         * A '/' a blob nevében virtuális mappát jelentene,
         * ezért fájlnévként lecseréljük.
         */
        var sanitized = fileName
            .Replace('/', '_')
            .Replace('\\', '_')
            .Trim();

        return string.IsNullOrWhiteSpace(sanitized)
            ? "document.xml"
            : sanitized;
    }

    private static string SanitizeMetadataValue(
        string value)
    {
        /*
         * A metadataértékeket egyszerű ASCII tartományra
         * korlátozzuk a problémamentes HTTP továbbítás érdekében.
         */
        return new string(
            [.. value.Select(character =>
                    character is >= ' ' and <= '~'
                        ? character
                        : '_')]);
    }
}