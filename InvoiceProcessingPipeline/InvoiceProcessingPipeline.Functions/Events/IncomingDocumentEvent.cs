using System.Text.Json;
using Azure.Messaging;
using Azure.Messaging.EventGrid.SystemEvents;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Functions.Orchestrators;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Events;

public sealed class IncomingDocumentEvent(ILogger<IncomingDocumentEvent> logger, IDocumentEventOrchestrator orchestratorService)
{
    [Function(nameof(IncomingDocumentEvent))]
    public async Task RunAsync(
        [EventGridTrigger] CloudEvent cloudEvent,
        [DurableClient] DurableTaskClient client,
        CancellationToken ct)
    {

        logger.LogInformation(
            "Received event. Id={EventId}, Type={EventType}, Subject={Subject}",
            cloudEvent.Id, cloudEvent.Type, cloudEvent.Subject);

        if (cloudEvent.Data is null)
        {
            logger.LogWarning("CloudEvent.Data is null");
            return;
        }

        var data = cloudEvent.Data.ToObjectFromJson<StorageBlobCreatedEventData>();

        logger.LogInformation(
            "Blob created. Url={Url}, ContentType={ContentType}, Size={Size}",
            data.Url, data.ContentType, data.Sequencer);

        var ingestionEvent = new IngestionEvent
        {
            CorrelationId = cloudEvent.Id, 

            EventId = cloudEvent.Id,
            EventType = cloudEvent.Type,
            Subject = cloudEvent.Subject ?? string.Empty,

            Body = cloudEvent.Data?.ToString() ?? string.Empty,

            EventTime = cloudEvent.Time?.UtcDateTime ?? DateTime.UtcNow,
            DataVersion = cloudEvent.DataSchema ?? string.Empty,

            Topic = string.Empty,
            TopicId = string.Empty
        };

        var instanceId = await orchestratorService.StartOrchestrationEventAsync(client, nameof(DocumentIngestionOrchestrator), ingestionEvent, ct);

        logger.LogInformation(
            "Started orchestration. InstanceId={InstanceId}, EventId={EventId}",
            instanceId, cloudEvent.Id);
    }
}