using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public interface IExtractedDocumentField
    {
        string? FieldName { get; }
        object? FieldOriginalContent { get; }
        double? ConfidenceScore { get; }

        DocumentField Extraction { get; }
        Type ExtractionType { get; }
    }
}