using InvoiceProcessingPipeline.Domain.ValueObjects;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;

[JsonObject(MemberSerialization.OptIn)]
public sealed record ExtractedDocumentField<TField>(
    [property: JsonProperty("extraction")] TField Extraction,
    [property: JsonProperty("fieldName")] string? FieldName = null,
    [property: JsonProperty("fieldOriginalContent")] object? FieldOriginalContent = null,
    [property: JsonProperty("confidenceScore")] double? ConfidenceScore = null)
    : IExtractedDocumentField
    where TField : DocumentField
{
    DocumentField IExtractedDocumentField.Extraction => Extraction;

    [JsonProperty("extractionType")]
    public Type ExtractionType => typeof(TField);
}