using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Functions.Activities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Orchestrators;

public sealed class DocumentIngestionOrchestrator
{
    [Function(nameof(DocumentIngestionOrchestrator))]
    public static async Task<DocumentStorageMetadata> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext ctx,
        DocumentIngestionEvent input)
    {
        var logger = ctx.CreateReplaySafeLogger(nameof(DocumentIngestionOrchestrator));
        logger.LogInformation("Document Ingestion Orchestrator started. CorrelationId={CorrelationId}", input.CorrelationId);

        // this activity is for recieving document metadata from BLOB storage for AI Document Intelligence processing
        var result = await ctx.CallActivityAsync<ActivityResult<DocumentStorageMetadata>>(nameof(RequestDocumentAccessibilityActivity), input);

        if (!result.IsSuccess)
        {
            // minimal: dobjunk exceptiont, hogy a durable failure legyen
            throw new InvalidOperationException($"{result.Error?.Code}: {result.Error?.Message}");
        }

        return result.Value!;
    }
}