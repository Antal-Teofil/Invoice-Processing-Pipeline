using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedDocumentField<TSource> where TSource : DocumentField
    {
        public string? FieldName { get; init; }

        public object? FieldOriginalContent { get; init; }

        public double? ConfidenceScore { get; init; }

        public TSource? ExtractedDocument { get; init; }

    }
}
