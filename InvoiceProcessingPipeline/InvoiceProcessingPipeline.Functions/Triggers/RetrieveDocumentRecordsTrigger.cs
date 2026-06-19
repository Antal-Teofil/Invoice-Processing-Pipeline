using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using static InvoiceProcessingPipeline.Application.Shared.ApplicationHttpExtensions;

namespace InvoiceProcessingPipeline.Functions.Triggers;

public sealed class RetrieveDocumentRecordsTrigger(
    ILogger<RetrieveDocumentRecordsTrigger> logger,
    IDocumentDataStore storage)
{
    private const int DefaultPageSize = 20;
    private const int MaximumPageSize = 100;

    [Function(nameof(RetrieveDocumentRecordsTrigger))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(
            AuthorizationLevel.Anonymous,
            "GET",
            Route = "audit/records")]
        HttpRequestData request,
        CancellationToken cancellationToken)
    {
        string? pageSizeParameter = request.Query["pageSize"];

        if (!TryGetPageSize(pageSizeParameter, out int pageSize))
        {
            return await request
                .BadRequest(
                    message:
                        $"The pageSize parameter must be between 1 and {MaximumPageSize}.",
                    code: "INVALID_PAGE_SIZE")
                .NoStore()
                .WithRequestId(request.FunctionContext.InvocationId)
                .BuildAsync(cancellationToken);
        }

        string? continuationToken =
            NormalizeContinuationToken(request.Query["continuationToken"]);

        logger.LogInformation(
            "Retrieving document records. PageSize: {PageSize}, HasContinuationToken: {HasContinuationToken}",
            pageSize,
            continuationToken is not null);

        PagedResult<CommercialInvoice> page =
            await storage.RetrievePagedDocumentCollectionAsync<CommercialInvoice>(
                pageSize,
                continuationToken,
                cancellationToken);

        IReadOnlyList<DocumentRecordInformation> records = [.. page.Data.Select(MapToDocumentRecordInformation)];

        PagedResult<DocumentRecordInformation> result = new()
        {
            Data = records,
            ContinuationToken = page.ContinuationToken
        };

        return await request
            .Ok(result)
            .WithRequestId(request.FunctionContext.InvocationId)
            .WithHeader("X-API-Version", "1.0")
            .BuildAsync(cancellationToken);
    }

    private static DocumentRecordInformation MapToDocumentRecordInformation(
        CommercialInvoice document)
    {
        return new DocumentRecordInformation
        {
            Header = new DocumentRecordHeader
            {
                DocumentAuditId = document.DocumentId.ToString(),

                // Stable placeholder. Later replace this with the real Durable workflow/process id.
                WorkflowId = document.DocumentId.ToString(),

                AuditStatus = document.AuditStatus,

                // Replace this later with real document UpdatedAt or Cosmos _ts.
                UpdatedAt = DateTimeOffset.UtcNow
            },

            Data = new DocumentRecordData
            {
                InvoiceNumber = document.InvoiceId?.Value,
                AccountingSupplierParty = document.AccountingSupplierParty?.Name?.Value,
                SupplierPhoneNumber = document.AccountingSupplierParty?.ContactInfo?.Telephone,
                SupplierEmailAddress = document.AccountingSupplierParty?.ContactInfo?.ElectronicMail,
                TotalAmount = document.LegalMonetaryTotal?.TaxInclusiveAmount.Amount,
                CurrencyCode = document.DocumentCurrencyCode?.Value,
                Auditor = "Teofil"
            }
        };
    }

    private static bool TryGetPageSize(
        string? value,
        out int pageSize)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            pageSize = DefaultPageSize;
            return true;
        }

        return int.TryParse(value, out pageSize)
            && pageSize is >= 1 and <= MaximumPageSize;
    }

    private static string? NormalizeContinuationToken(
        string? continuationToken) =>
        string.IsNullOrWhiteSpace(continuationToken)
            ? null
            : continuationToken;
}