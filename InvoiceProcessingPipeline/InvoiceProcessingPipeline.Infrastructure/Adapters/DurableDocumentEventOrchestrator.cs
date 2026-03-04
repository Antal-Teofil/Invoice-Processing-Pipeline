using InvoiceProcessingPipeline.Application.Auditing;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class DurableDocumentEventOrchestrator(ILogger<DurableDocumentEventOrchestrator> logger, DocumentAuditOrchestrator documentOrchestrator) : IDocumentEventOrchestrator
    {
        public async Task OrchestrateEventAsync(DurableTaskClient client, string orchestratorName, DocumentIngestionEvent docEvent, CancellationToken token)
        {
            var status = await documentOrchestrator.RecordDocumentIngestionEventAsync(docEvent, token);

            if(status.Value is null || status.Value.Exists is true)
            {
                return;
            }
            logger.LogInformation("Scheduling orchestration for document with CorrelationId: {CorrelationId}", docEvent.CorrelationId);
            await client.ScheduleNewOrchestrationInstanceAsync(orchestratorName, docEvent, token);
        }
    }
}
