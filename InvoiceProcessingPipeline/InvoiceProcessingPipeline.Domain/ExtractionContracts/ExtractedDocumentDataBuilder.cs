using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.ExtractionContracts;

/// <summary>
/// this builder class is for extracting arbitrary amount of field from a given source
/// </summary>
public sealed class ExtractedDocumentDataBuilder
{
    /// <summary>
    /// information about the api used for extracting data
    /// </summary>
    private AnalyzerInformation? _analyzerInformation;

    private string _documentId = Guid.NewGuid().ToString();

    private string? _processId;

    /// <summary>
    /// a dictionary which contains the canonicalized extracted fields
    /// </summary>
    private readonly ExtractedDocumentFieldDictionary _fieldDictionary = new();

    public ExtractedDocumentDataBuilder WithAnalyzerInformation(
        AnalyzerInformation analyzerInformation)
    {
        ArgumentNullException.ThrowIfNull(analyzerInformation);

        _analyzerInformation = analyzerInformation;
        return this;
    }

    public ExtractedDocumentDataBuilder WithProcessId(string processId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(processId);

        _processId = processId;
        return this;
    }

    public ExtractedDocumentDataBuilder WithDocumentId(string documentId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);

        _documentId = documentId;
        return this;
    }

    /// <summary>
    /// it is a higher-order function which takes a string and a function as inputs and must return a canonicalized extracted field
    /// </summary>
    public ExtractedDocumentDataBuilder ExtractFieldAs<TField>(
        string givenFieldName,
        Func<ExtractedDocumentField<TField>> extractor)
        where TField : DocumentField
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(givenFieldName);
        ArgumentNullException.ThrowIfNull(extractor);

        var extractedField = extractor();

        if (extractedField is not null)
        {
            _fieldDictionary.AddField(givenFieldName, extractedField);
        }

        return this;
    }

    public ExtractedDocumentDataBuilder AddField<TField>(
        string givenFieldName,
        ExtractedDocumentField<TField> field)
        where TField : DocumentField =>
        ExtractFieldAs(givenFieldName, () => field);

    public ExtractedDocumentData Build()
    {
        if (string.IsNullOrWhiteSpace(_processId))
        {
            throw new InvalidOperationException("ProcessId must be set before building.");
        }

        return new ExtractedDocumentData(
            _analyzerInformation,
            _fieldDictionary,
            _documentId,
            _processId);
    }
}