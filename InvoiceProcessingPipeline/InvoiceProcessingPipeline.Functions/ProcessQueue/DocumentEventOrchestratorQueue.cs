using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.ProcessQueue;

public sealed class DocumentEventOrchestratorQueue(
    ILogger<DocumentEventOrchestratorQueue> logger,
    IDocumentEventOrchestrator docOrchestrator)
{
    [Function(nameof(DocumentEventOrchestratorQueue))]
    public async Task RunAsync(
        [QueueTrigger("document-processing", Connection = "QUEUE_STORAGE")] DocumentIngestionEvent docEvent,
        [DurableClient] DurableTaskClient client,
        CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(docEvent);

            logger.LogInformation(
                "Processing document event. EventId: {EventId}, CorrelationId: {CorrelationId}, DocumentUrl: {DocumentUrl}",
                docEvent.EventMetadata.EventId,
                docEvent.CorrelationId,
                docEvent.StorageMetadata.DocumentUrl);

            var inserted = await docOrchestrator.TryRecordEventAsync(docEvent, cancellationToken);

            if (!inserted)
            {
                logger.LogInformation(
                    "Skipping orchestration because the event is duplicate. EventId: {EventId}",
                    docEvent.EventMetadata.EventId);

                return;
            }

            const string orchestratorName = "DocumentIngestionOrchestrator";

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                orchestratorName,
                docEvent,
                cancellationToken);

            var process = new DocumentOrchestrationTask(
                new DocumentOrchestrationTaskID(instanceId),
                orchestratorName,
                DateTimeOffset.UtcNow,
                docEvent);

            await docOrchestrator.RecordDocumentOrchestrationEventAsync(process, cancellationToken);

            logger.LogInformation(
                "Document orchestration started successfully. OrchestrationId: {OrchestrationId}, EventId: {EventId}",
                instanceId,
                docEvent.EventMetadata.EventId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DocumentEventOrchestratorQueue failed.");
            throw;
        }
    }
}