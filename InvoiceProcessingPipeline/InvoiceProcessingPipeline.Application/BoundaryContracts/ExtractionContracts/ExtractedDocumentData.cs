using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;

public sealed class ExtractedDocumentData(
    AnalyzerInformation? analyzerInformation,
    ExtractedDocumentFieldDictionary fieldDictionary,
    string documentId)
{
    public string Id { get; } = documentId;
    public AnalyzerInformation? AnalyzerInformation { get; } = analyzerInformation;

    public ExtractedDocumentFieldDictionary FieldDictionary { get; } = fieldDictionary.Clone();

    public bool TryGetField<TField>(
        string fieldName,
        out ExtractedDocumentField<TField>? field)
        where TField : DocumentField =>
        FieldDictionary.TryGet(fieldName, out field);
}