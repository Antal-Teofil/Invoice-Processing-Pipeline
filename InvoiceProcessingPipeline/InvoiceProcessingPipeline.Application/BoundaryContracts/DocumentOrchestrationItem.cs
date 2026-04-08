using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentOrchestrationItem
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    public required string PartitionKey { get; init; }
    public required string OrchestratorName { get; init; }
    public required string EventId { get; init; }
    public required string CorrelationId { get; init; }
    public required string DocumentUrl { get; init; }
    public required DateTimeOffset StartedAtUtc { get; init; }
}