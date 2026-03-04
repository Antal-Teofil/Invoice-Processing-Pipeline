using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Functions.Worker;

namespace InvoiceProcessingPipeline.Functions.Activities;

public sealed class RequestDocumentMetadataActivity
{
    [Function(nameof(RequestDocumentMetadataActivity))]
    public Task<ActivityResult<DocumentStorageMetadata>> RunAsync([ActivityTrigger] DocumentIngestionEvent ingestionEvent)
    {
        throw new NotImplementedException();
    }
}