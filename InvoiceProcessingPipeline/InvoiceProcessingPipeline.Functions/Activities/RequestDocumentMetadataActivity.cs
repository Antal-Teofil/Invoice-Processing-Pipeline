using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Functions.Worker;

namespace InvoiceProcessingPipeline.Functions.Activities;

public sealed class RequestDocumentMetadataActivity
{
    [Function(nameof(RequestDocumentMetadataActivity))]
    public Task<ActivityResult<DocumentMetadata>> RunAsync([ActivityTrigger] IngestionEvent ingestionEvent)
    {
        var response = new DocumentMetadata
        {
            CorrelationId = ingestionEvent.CorrelationId,
            DocumentId = Guid.NewGuid().ToString(),
            FileName = "sample.pdf",
            ContentType = "application/pdf",
            Size = 1024
        };

        return Task.FromResult(ActivityResult<DocumentMetadata>.Success(response));
    }
}