using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Orchestrators;

public sealed class DocumentIngestionOrchestrator
{
    [Function(nameof(DocumentIngestionOrchestrator))]
    public async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext ctx,
        DocumentIngestionEvent input)
    {
        ArgumentNullException.ThrowIfNull(ctx);
        ArgumentNullException.ThrowIfNull(input);

        var logger = ctx.CreateReplaySafeLogger(nameof(DocumentIngestionOrchestrator));

        logger.LogInformation(
            "DocumentIngestionOrchestrator started. CorrelationId: {CorrelationId}, EventId: {EventId}",
            input.CorrelationId,
            input.EventMetadata.EventId);

        ActivityResult<DocumentSasUri> sasResult =
            await ctx.CallActivityAsync<ActivityResult<DocumentSasUri>>(
                nameof(Activities.RequestDocumentAccessibilityActivity),
                input);

        if (!sasResult.IsSuccess || sasResult.Value is null)
        {
            throw new InvalidOperationException(
                sasResult.ErrorMessage ?? "RequestDocumentAccessibilityActivity failed.");
        }

        logger.LogInformation(
            "Document SAS generation completed successfully. CorrelationId: {CorrelationId}",
            input.CorrelationId);

        // Ide jönnek a további activity-k.
        // Példa:
        // var extracted = await ctx.CallActivityAsync<...>(
        //     nameof(Activities.ExtractDocumentDataActivity),
        //     input);
    }
}