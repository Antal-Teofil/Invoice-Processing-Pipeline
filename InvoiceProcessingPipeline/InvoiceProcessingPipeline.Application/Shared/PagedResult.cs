using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.Shared;

public sealed record PagedResult<T>
    where T : class
{
    [JsonPropertyName("items")]
    public required IReadOnlyList<T> Data { get; init; }

    [JsonPropertyName("continuationToken")]
    public string? ContinuationToken { get; init; }

    [JsonPropertyName("hasMore")]
    public bool HasMore =>
        !string.IsNullOrWhiteSpace(ContinuationToken);
}