using InvoiceProcessingPipeline.Domain.ValueObjects;
using Newtonsoft.Json;

namespace InvoiceProcessingPipeline.Domain.ExtractionContracts;

[JsonObject(MemberSerialization.OptIn)]
public sealed class ExtractedDocumentData(
    AnalyzerInformation? analyzerInformation,
    ExtractedDocumentFieldDictionary fieldDictionary,
    string documentId,
    string processId)
{
    [JsonProperty("id")]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    [JsonProperty("documentId")]
    public string DocumentId { get; } = documentId;

    [JsonProperty("processId")]
    public string? ProcessId { get; } = processId;

    [JsonProperty("analyzerInformation")]
    public AnalyzerInformation? AnalyzerInformation { get; } = analyzerInformation;

    [JsonProperty("fieldDictionary", ItemConverterType = typeof(ExtractedDocumentFieldJsonConverter))]
    public ExtractedDocumentFieldDictionary FieldDictionary { get; } = fieldDictionary.Clone();

    public bool TryGetField<TField>(
        string fieldName,
        out ExtractedDocumentField<TField>? field)
        where TField : DocumentField =>
        FieldDictionary.TryGet(fieldName, out field);


}
