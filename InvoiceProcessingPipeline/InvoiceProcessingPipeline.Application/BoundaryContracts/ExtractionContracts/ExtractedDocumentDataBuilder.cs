using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;

public sealed class ExtractedDocumentDataBuilder
{
    private AnalyzerInformation? _analyzerInformation;
    private readonly ExtractedDocumentFieldDictionary _fieldDictionary = new();

    public ExtractedDocumentDataBuilder WithAnalyzerInformation(
        AnalyzerInformation analyzerInformation)
    {
        ArgumentNullException.ThrowIfNull(analyzerInformation);

        _analyzerInformation = analyzerInformation;
        return this;
    }

    public ExtractedDocumentDataBuilder ExtractFieldAs<TField>(
        string givenFieldName,
        Func<ExtractedDocumentField<TField>> extractor)
        where TField : DocumentField
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(givenFieldName);
        ArgumentNullException.ThrowIfNull(extractor);

        var extractedField = extractor()
            ?? throw new InvalidOperationException(
                $"Az extractor null értéket adott vissza a(z) '{givenFieldName}' mezőhöz.");

        var normalizedField = extractedField with
        {
            FieldName = extractedField.FieldName ?? givenFieldName
        };

        _fieldDictionary.Add(givenFieldName, normalizedField);

        return this;
    }

    public ExtractedDocumentDataBuilder AddField<TField>(
        string givenFieldName,
        ExtractedDocumentField<TField> field)
        where TField : DocumentField =>
        ExtractFieldAs(givenFieldName, () => field);

    public ExtractedDocumentData Build() =>
        new(_analyzerInformation, _fieldDictionary);
}