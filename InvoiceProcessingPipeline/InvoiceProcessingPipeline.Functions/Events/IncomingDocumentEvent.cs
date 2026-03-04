using Azure.Messaging;
using Azure.Messaging.EventGrid.SystemEvents;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Functions.Orchestrators;
using Mapster;
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

        logger.LogInformation("Received EventGrid CloudEvent with Id: {EventId}, Type: {EventType}, Source: {EventSource}", cloudEvent.Id, cloudEvent.Type, cloudEvent.Source);
        logger.LogInformation("Recieved EventGrid CloudEvent");

        var blobData = cloudEvent.Data?.ToObjectFromJson<StorageBlobCreatedEventData>();

        var documentMetadata = blobData.Adapt<DocumentStorageMetadata>();
        var documentEventMetadata = cloudEvent.Adapt<DocumentEventMetadata>();

        DocumentIngestionEvent documentIngestionEvent = new()
        {
            CorrelationId = Guid.NewGuid().ToString(),
            StorageMetadata = documentMetadata,
            EventMetadata = documentEventMetadata
        };

        // valami normalis visszateritesi erteket ki kene talalni.

        await orchestratorService.OrchestrateEventAsync(client, nameof(DocumentIngestionOrchestrator), documentIngestionEvent, ct);
    }
}