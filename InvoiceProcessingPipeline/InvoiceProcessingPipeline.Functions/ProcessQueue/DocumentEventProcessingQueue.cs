using Castle.Core.Logging;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.ProcessQueue
{
    public sealed class DocumentEventProcessingQueue(ILogger<DocumentEventProcessingQueue> logger, IDocumentEventOrchestrator docOrchestrator)
    {
        [Function(nameof(DocumentEventProcessingQueue))]
        public async Task RunAsync([QueueTrigger("document-processing")] DocumentIngestionEvent docEvent)
        {
            await docOrchestrator.RecordEventAsync(docEvent);
            DocumentOrchestrationProcessID id = await docOrchestrator.StartDocumentOrchestrationAsync();
            DocumentOrchestrationProcess process = new(id, docEvent);
            await docOrchestrator.RecordDocumentOrchestrationEvent(process);
        }
    }
}
