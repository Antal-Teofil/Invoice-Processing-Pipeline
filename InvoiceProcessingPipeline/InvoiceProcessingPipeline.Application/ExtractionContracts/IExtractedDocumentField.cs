using InvoiceProcessingPipeline.Application.MapperConfigurations;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using Newtonsoft.Json;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    [JsonConverter(typeof(ExtractedDocumentFieldJsonConverter))]
    public interface IExtractedDocumentField
    {
        string? FieldName { get; }
        object? FieldOriginalContent { get; }
        double? ConfidenceScore { get; }

        DocumentField Extraction { get; }
        Type ExtractionType { get; }
    }
}