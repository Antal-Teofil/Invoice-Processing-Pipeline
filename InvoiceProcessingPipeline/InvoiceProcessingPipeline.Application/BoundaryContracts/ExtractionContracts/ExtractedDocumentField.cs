using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedDocumentField<TField>(
    TField Extraction,
    string? FieldName = null,
    object? FieldOriginalContent = null,
    double? ConfidenceScore = null)
    : IExtractedDocumentField
    where TField : DocumentField
    {
        DocumentField IExtractedDocumentField.Extraction => Extraction;

        public Type ExtractionType => typeof(TField);
    }
}