using InvoiceProcessingPipeline.Domain.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InvoiceProcessingPipeline.Domain.ExtractionContracts;

public sealed class ExtractedDocumentFieldJsonConverter : JsonConverter<IExtractedDocumentField>
{
    public override IExtractedDocumentField? ReadJson(
        JsonReader reader,
        Type objectType,
        IExtractedDocumentField? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);

        string extractionTypeName = obj.Value<string>("extractionType")
            ?? throw new JsonSerializationException("Missing extractionType.");

        Type extractionClrType = Type.GetType(extractionTypeName, throwOnError: true)!
            ?? throw new JsonSerializationException($"Unknown extractionType: {extractionTypeName}");

        if (!typeof(DocumentField).IsAssignableFrom(extractionClrType))
        {
            throw new JsonSerializationException(
                $"Type {extractionTypeName} is not a DocumentField.");
        }

        JToken extractionToken = obj["extraction"]
            ?? throw new JsonSerializationException("Missing extraction.");

        DocumentField extraction = (DocumentField)(
            extractionToken.ToObject(extractionClrType, serializer)
            ?? throw new JsonSerializationException(
                $"Could not deserialize extraction as {extractionClrType.FullName}.")
        );

        string? fieldName = obj.Value<string>("fieldName");
        object? fieldOriginalContent = obj["fieldOriginalContent"]?.ToObject<object>(serializer);
        double? confidenceScore = obj.Value<double?>("confidenceScore");

        Type wrapperType = typeof(ExtractedDocumentField<>).MakeGenericType(extractionClrType);

        return (IExtractedDocumentField?)Activator.CreateInstance(
            wrapperType,
            extraction,
            fieldName,
            fieldOriginalContent,
            confidenceScore);
    }

    public override void WriteJson(
        JsonWriter writer,
        IExtractedDocumentField? value,
        JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        JObject obj = new()
        {
            ["extraction"] = JToken.FromObject(value.Extraction, serializer),
            ["fieldName"] = value.FieldName is null ? JValue.CreateNull() : JToken.FromObject(value.FieldName, serializer),
            ["fieldOriginalContent"] = value.FieldOriginalContent is null ? JValue.CreateNull() : JToken.FromObject(value.FieldOriginalContent, serializer),
            ["confidenceScore"] = value.ConfidenceScore is null ? JValue.CreateNull() : JToken.FromObject(value.ConfidenceScore, serializer),
            ["extractionType"] = value.ExtractionType.AssemblyQualifiedName
        };

        obj.WriteTo(writer);
    }
}