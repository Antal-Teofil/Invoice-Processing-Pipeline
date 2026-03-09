using InvoiceProcessingPipeline.Application.BoundaryContracts;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentEventOrchestrator
    {
        public Task RecordEvent(DocumentIngestionEvent eventRecord);
    }
}
