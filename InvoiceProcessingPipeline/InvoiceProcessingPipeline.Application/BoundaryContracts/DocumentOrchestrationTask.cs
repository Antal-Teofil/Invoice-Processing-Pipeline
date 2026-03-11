namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentOrchestrationTask(
    DocumentOrchestrationTaskID Id,
    string OrchestratorName,
    DateTimeOffset StartedAtUtc,
    DocumentIngestionEvent DocumentEvent);