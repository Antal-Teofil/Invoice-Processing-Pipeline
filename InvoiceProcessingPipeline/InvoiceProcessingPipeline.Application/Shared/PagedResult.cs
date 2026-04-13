namespace InvoiceProcessingPipeline.Application.Shared
{
    public sealed record PagedResult<T>(
    IReadOnlyCollection<T> Items,
    string? ContinuationToken);
}
