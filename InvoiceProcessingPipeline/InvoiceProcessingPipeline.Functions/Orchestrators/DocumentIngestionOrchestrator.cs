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
    public static async Task<DocumentMetadata> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext ctx,
        IngestionEvent input)
    {
        var logger = ctx.CreateReplaySafeLogger(nameof(DocumentIngestionOrchestrator));
        logger.LogInformation("Document Ingestion Orchestrator started. CorrelationId={CorrelationId}", input.CorrelationId);

        var result = await ctx.CallActivityAsync<ActivityResult<DocumentMetadata>>(
            nameof(RequestDocumentMetadataActivity),
            input);

        if (!result.IsSuccess)
        {
            // minimal: dobjunk exceptiont, hogy a durable failure legyen
            throw new InvalidOperationException($"{result.Error?.Code}: {result.Error?.Message}");
        }

        return result.Value!;
    }
}