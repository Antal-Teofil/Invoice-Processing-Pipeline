using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;

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

    /// <summary>
    /// it is a higher-order function which takes a string and a function as inputs and must return a canonicalized extrcated field
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    /// <param name="givenFieldName"></param>
    /// <param name="extractor"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ExtractedDocumentDataBuilder ExtractFieldAs<TField>(
        string givenFieldName,
        Func<ExtractedDocumentField<TField>> extractor)
        where TField : DocumentField
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(givenFieldName);
        ArgumentNullException.ThrowIfNull(extractor);

        var extractedField = extractor();
        
        if(extractedField is not null)
        {
            _fieldDictionary.Add(givenFieldName, extractedField);
        }

        return this;
    }

    public ExtractedDocumentDataBuilder AddField<TField>(
        string givenFieldName,
        ExtractedDocumentField<TField> field)
        where TField : DocumentField =>
        ExtractFieldAs(givenFieldName, () => field);

    public ExtractedDocumentData Build() =>
        new(_analyzerInformation, _fieldDictionary, _documentId);
}