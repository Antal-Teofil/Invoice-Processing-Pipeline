using InvoiceProcessingPipeline.Application.BoundaryContracts;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentEventOrchestrator
    {
        public Task RecordEventAsync(DocumentIngestionEvent eventRecord);
        public ValueTask<DocumentIngestionEvent?> RetrieveEventRecordAsync(EventID Id);
        public ValueTask<bool> VerifyEventRecordExistanceAsync(EventID Id);
        public ValueTask<DocumentOrchestrationTaskID> StartDocumentOrchestrationAsync(string TaskName, );
        public Task RecordDocumentOrchestrationEvent(DocumentOrchestrationTask docProcess);
    }
}
