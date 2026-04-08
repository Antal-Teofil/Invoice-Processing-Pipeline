using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts;

public sealed record DocumentEventItem
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    public required string PartitionKey { get; init; }
    public required DateTimeOffset RecordedAtUtc { get; init; }
    public required DocumentIngestionEvent Payload { get; init; }
}