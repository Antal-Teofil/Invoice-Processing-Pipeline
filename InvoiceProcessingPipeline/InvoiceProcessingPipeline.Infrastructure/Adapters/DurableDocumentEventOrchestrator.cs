using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class DurableDocumentEventOrchestrator(ILogger<DurableDocumentEventOrchestrator> logger) : IDocumentEventOrchestrator
    {
        public async Task<string> OrchestrateEventAsync(DurableTaskClient client, string orchestratorName, DocumentIngestionEvent docEvent, CancellationToken token)
        {
            return await client.ScheduleNewOrchestrationInstanceAsync(orchestratorName, docEvent, token);
        }
    }
}
