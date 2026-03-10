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

public sealed class IncomingDocumentEvent(ILogger<IncomingDocumentEvent> logger)
{
    [Function(nameof(IncomingDocumentEvent))]
    [QueueOutput("document-processing")]
    public async Task<DocumentIngestionEvent> RunAsync([EventGridTrigger] CloudEvent cloudEvent)
    {

        logger.LogInformation("Received EventGrid CloudEvent with Id: {EventId}, Type: {EventType}, Source: {EventSource}", cloudEvent.Id, cloudEvent.Type, cloudEvent.Source);
        logger.LogInformation("Recieved EventGrid CloudEvent");

        var blobData = cloudEvent.Data?.ToObjectFromJson<StorageBlobCreatedEventData>();

        // to be verified, event id null, url null
        // correct mapster mapping
        var documentMetadata = blobData.Adapt<DocumentStorageMetadata>();
        var documentEventMetadata = cloudEvent.Adapt<DocumentEventMetadata>();

        DocumentIngestionEvent documentIngestionEvent = new()
        {
            Id = Guid.NewGuid().ToString(),
            CorrelationId = Guid.NewGuid().ToString(),
            StorageMetadata = documentMetadata,
            EventMetadata = documentEventMetadata
        };

        return documentIngestionEvent;
    }
}