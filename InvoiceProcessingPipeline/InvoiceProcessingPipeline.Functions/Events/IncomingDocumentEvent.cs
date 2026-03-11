using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Azure.Storage.Queues;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using MapsterMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvoiceProcessingPipeline.Functions.Events;

public sealed class IncomingDocumentEvent(
    ILogger<IncomingDocumentEvent> logger,
    IMapper mapper,
    QueueClient queueClient)
{
    [Function(nameof(IncomingDocumentEvent))]
    public async Task RunAsync([EventGridTrigger] CloudEvent cloudEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cloudEvent);

        logger.LogInformation(
            "Received CloudEvent. Id: {EventId}, Type: {EventType}, Source: {EventSource}",
            cloudEvent.Id,
            cloudEvent.Type,
            cloudEvent.Source);

        if (!string.Equals(cloudEvent.Type, SystemEventNames.StorageBlobCreated, StringComparison.Ordinal))
        {
            throw new NotSupportedException(
                $"Unsupported event type '{cloudEvent.Type}'. Expected '{SystemEventNames.StorageBlobCreated}'.");
        }

        if (cloudEvent.Data is null)
        {
            throw new InvalidOperationException("CloudEvent.Data is null.");
        }

        StorageBlobCreatedEventData blobData;
        try
        {
            blobData = cloudEvent.Data.ToObjectFromJson<StorageBlobCreatedEventData>()
                ?? throw new InvalidOperationException(
                    $"CloudEvent.Data could not be deserialized to {nameof(StorageBlobCreatedEventData)}.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"CloudEvent.Data is not a valid {nameof(StorageBlobCreatedEventData)} payload.",
                ex);
        }

        var storageMetadata = mapper.Map<DocumentStorageMetadata>(blobData);
        var eventMetadata = mapper.Map<DocumentEventMetadata>(cloudEvent);

        var documentIngestionEvent = new DocumentIngestionEvent
        {
            Id = eventMetadata.EventId,
            CorrelationId = eventMetadata.EventId,
            StorageMetadata = storageMetadata,
            EventMetadata = eventMetadata
        };

        var payload = JsonSerializer.Serialize(documentIngestionEvent);

        await queueClient.SendMessageAsync(payload, cancellationToken);

        logger.LogInformation(
            "Canonical DocumentIngestionEvent enqueued. EventId: {EventId}, DocumentUrl: {DocumentUrl}",
            documentIngestionEvent.EventMetadata.EventId,
            documentIngestionEvent.StorageMetadata.DocumentUrl);
    }
}