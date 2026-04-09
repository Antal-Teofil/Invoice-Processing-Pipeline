using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Application.Validations;
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

        // itt hivodik meg az activity ami eloallitja a user delegation sas uri-t
        ActivityResult<DocumentUserDelegationSasUri> sasResult =
            await ctx.CallActivityAsync<ActivityResult<DocumentUserDelegationSasUri>>(
                nameof(Activities.RequestDocumentAccessibilityActivity),
                input);

        // ez dolgozza fel a beerkezo BLOB-ot
        ActivityResult<ExtractedDocumentResponse> rawDocData =
            await ctx.CallActivityAsync<ActivityResult<ExtractedDocumentResponse>>(nameof(Activities.ExtractDocumentDataActivity), sasResult.Value);

        ActivityResult<Accumulator<SchemaViolation>> schemaValidationResult =
            await ctx.CallActivityAsync<ActivityResult<Accumulator<SchemaViolation>>>(nameof(Activities.AnalyzeSchemaIntegrityActivity), rawDocData);



        // Ide jönnek a további activity-k.
        // Példa:.
        // var extracted = await ctx.CallActivityAsync<...>(
        //     nameof(Activities.ExtractDocumentDataActivity),
        //     input);
    }
}