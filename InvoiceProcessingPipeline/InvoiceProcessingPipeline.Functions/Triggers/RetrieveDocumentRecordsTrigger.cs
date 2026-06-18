using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using MapsterMapper;
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
            NormalizeContinuationToken(
                request.Query["continuationToken"]);

        logger.LogInformation(
            "Retrieving document records. PageSize: {PageSize}, HasContinuationToken: {HasContinuationToken}",
            pageSize,
            continuationToken is not null);

        PagedResult<CommercialInvoice> page = await storage.RetrievePagedDocumentCollectionAsync<CommercialInvoice>(
            pageSize,
            continuationToken,
            cancellationToken);

        IReadOnlyList<DocumentRecordInformation> records = [.. page.Data
        .Select(document => new DocumentRecordInformation
        {
            AuditStatus = Enum.Parse<AuditStatus>(document.AuditStatus),
            ProcessId = Guid.NewGuid().ToString(), // TODO: to be replaced
            DocumentId = document.DocumentId.ToString(),
            InvoiceId = document?.InvoiceId?.Value,
            VendorName = document?.AccountingSupplierParty?.Name?.Value,
            PhoneNumber = document?.AccountingSupplierParty?.ContactInfo?.Telephone,
            VendorEmailAddress = document?.AccountingSupplierParty?.ContactInfo?.ElectronicMail,
            TotalAmount = document?.LegalMonetaryTotal?.TaxInclusiveAmount.Amount,
            CurrencyCode = document?.DocumentCurrencyCode?.Value,
            UpdatedAt = DateTimeOffset.Now,
            ReviewedBy = "Teofil"
        })];

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