using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;


namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class DurableDocumentEventOrchestrator(ILogger<DurableDocumentEventOrchestrator> logger) : IDocumentEventOrchestrator
    {
        public async Task<string> StartOrchestrationEventAsync(DurableTaskClient client, string orchestratorName, BoundaryContract cntr, CancellationToken token)
        {
            logger.LogInformation("Starting orchestration {orchestratorName} with correlation id {correlationId}", orchestratorName, cntr.CorrelationId);
            var instaceId = await client.ScheduleNewOrchestrationInstanceAsync(orchestratorName, cntr, token);
            return instaceId;
        }
    }
}
