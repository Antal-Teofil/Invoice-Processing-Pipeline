using System.Net;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters;

public sealed class DocumentEventOrchestratorService(
    ILogger<DocumentEventOrchestratorService> logger,
    [FromKeyedServices("invoice-event")] Container eventContainer,
    [FromKeyedServices("document-orchestration")] Container orchestrationContainer)
    : IDocumentEventOrchestrator
{
    public async Task<bool> TryRecordEventAsync(
        DocumentIngestionEvent eventRecord,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(eventRecord);

        var eventId = eventRecord.EventMetadata.EventId;

        var item = new DocumentEventItem
        {
            Id = eventId,
            PartitionKey = eventId,
            RecordedAtUtc = DateTimeOffset.UtcNow,
            Payload = eventRecord
        };

        try
        {
            await eventContainer.CreateItemAsync(
                item,
                new PartitionKey(item.PartitionKey),
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Document event recorded. EventId: {EventId}, DocumentUrl: {DocumentUrl}",
                eventRecord.EventMetadata.EventId,
                eventRecord.StorageMetadata.DocumentUrl);

            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogInformation(
                "Duplicate document event detected. EventId: {EventId}",
                eventRecord.EventMetadata.EventId);

            return false;
        }
    }

    public async Task<DocumentIngestionEvent?> RetrieveEventRecordAsync(
        string eventId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventId);

        try
        {
            var response = await eventContainer.ReadItemAsync<DocumentEventItem>(
                id: eventId,
                partitionKey: new PartitionKey(eventId),
                cancellationToken: cancellationToken);

            return response.Resource.Payload;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> VerifyEventRecordExistenceAsync(
        string eventId,
        CancellationToken cancellationToken = default)
    {
        var result = await RetrieveEventRecordAsync(eventId, cancellationToken);
        return result is not null;
    }

    public async Task RecordDocumentOrchestrationEventAsync(
        DocumentOrchestrationTask docProcess,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(docProcess);

        var item = new DocumentOrchestrationItem
        {
            Id = docProcess.Id.Id,
            PartitionKey = docProcess.Id.Id,
            OrchestratorName = docProcess.OrchestratorName,
            EventId = docProcess.DocumentEvent.EventMetadata.EventId,
            CorrelationId = docProcess.DocumentEvent.CorrelationId,
            DocumentUrl = docProcess.DocumentEvent.StorageMetadata.DocumentUrl,
            StartedAtUtc = docProcess.StartedAtUtc
        };

        await orchestrationContainer.CreateItemAsync(
            item,
            new PartitionKey(item.PartitionKey),
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Document orchestration record created. OrchestrationId: {OrchestrationId}, EventId: {EventId}",
            docProcess.Id.Id,
            docProcess.DocumentEvent.EventMetadata.EventId);
    }
}