using InvoiceProcessingPipeline.Application.BoundaryContracts;

namespace InvoiceProcessingPipeline.Application.Ports;

public interface IDocumentEventOrchestrator
{
    Task<bool> TryRecordEventAsync(
        DocumentIngestionEvent eventRecord,
        CancellationToken cancellationToken = default);

    Task<DocumentIngestionEvent?> RetrieveEventRecordAsync(
        string eventId,
        CancellationToken cancellationToken = default);

    Task<bool> VerifyEventRecordExistenceAsync(
        string eventId,
        CancellationToken cancellationToken = default);

    Task RecordDocumentOrchestrationEventAsync(
        DocumentOrchestrationTask docProcess,
        CancellationToken cancellationToken = default);
}