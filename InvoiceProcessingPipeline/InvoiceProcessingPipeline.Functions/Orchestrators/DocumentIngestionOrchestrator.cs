using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.DocumentAudit;
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
        //=======================================================================================================
        var logger = ctx.CreateReplaySafeLogger(nameof(DocumentIngestionOrchestrator));

        logger.LogInformation(
            "DocumentIngestionOrchestrator started. CorrelationId: {CorrelationId}, EventId: {EventId}",
            input.CorrelationId,
            input.EventMetadata.EventId);

        // requests a user delegation SAS URI for the document to be ingested
        ActivityResult<DocumentUserDelegationSasUri> sasResult =
            await ctx.CallActivityAsync<ActivityResult<DocumentUserDelegationSasUri>>(
                nameof(Activities.RequestDocumentAccessibilityActivity),
                input);

        ActivityInput acInput = new()
        {
            SasUri = sasResult?.Value,
            ProcessId = ctx.InstanceId,
        };
        // calls the activity responsible for extracting data from the document
        ActivityResult<ExtractedDocumentResponse> rawDocData =
            await ctx.CallActivityAsync<ActivityResult<ExtractedDocumentResponse>>(nameof(Activities.ExtractDocumentDataActivity), acInput);

        DocumentAuditSnapshot extractionSnapshot = new()
        {
            DocumentId = rawDocData?.Value?.ExtractedDocumentId,
            OrchestrationId = ctx.InstanceId,
            AuditStatus = AuditStatus.EXTRACTED,
        };
        ctx.SetCustomStatus(extractionSnapshot);

        // calls the activity responsible for canonicalizing the extracted data
        ActivityResult<string> documentId =
            await ctx.CallActivityAsync<ActivityResult<string>>(nameof(Activities.DocumentSchemeCanonicalizerActivity), rawDocData?.Value?.ExtractedDocumentId);

        DocumentAuditSnapshot canonicalization = new()
        {
            DocumentId = rawDocData?.Value?.ExtractedDocumentId,
            OrchestrationId = ctx.InstanceId,
            AuditStatus = AuditStatus.UNDER_REVIEW
        };

        ctx.SetCustomStatus(canonicalization);

        // calls the activity responsible for analyzing the integrity of the extracted data by applying a set of constraints
        string extractedDocumentId = rawDocData?.Value?.ExtractedDocumentId ?? string.Empty;

        do
        {
            ActivityResult<bool> anyIssue =
                await ctx.CallActivityAsync<ActivityResult<bool>>(nameof(Activities.AnalyzeConstraintIntegrityActivity), extractedDocumentId);

            if (anyIssue.Value)
            {
                DocumentAuditSnapshot constrainViolation = new()
                {
                    DocumentId = rawDocData?.Value?.ExtractedDocumentId,
                    OrchestrationId = ctx.InstanceId,
                    AuditStatus = AuditStatus.CONSTRAINT_VIOLATION
                };

                ctx.SetCustomStatus(constrainViolation);

                string correctedDocumentId = await ctx.WaitForExternalEvent<string>(nameof(ExtractionCorrectionSubmitted));
            } else
            {
                DocumentAuditSnapshot extractionSuccess = new()
                {
                    DocumentId = rawDocData?.Value?.ExtractedDocumentId,
                    OrchestrationId = ctx.InstanceId,
                    AuditStatus = AuditStatus.APPROVED
                };

                ctx.SetCustomStatus(extractionSuccess);
                break;
            }
        } while(true);


    }
}