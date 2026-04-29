using InvoiceProcessingPipeline.Domain.ValueObjects;
using Newtonsoft.Json;

namespace InvoiceProcessingPipeline.Domain.ExtractionContracts;

[JsonDictionary(ItemConverterType = typeof(ExtractedDocumentFieldJsonConverter))]
public sealed class ExtractedDocumentFieldDictionary
    : Dictionary<string, IExtractedDocumentField>
{
    public ExtractedDocumentFieldDictionary()
        : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public ExtractedDocumentFieldDictionary(
        IDictionary<string, IExtractedDocumentField> items)
        : base(items, StringComparer.OrdinalIgnoreCase)
    {
    }

    public bool TryGet<TField>(string key, out ExtractedDocumentField<TField>? value)
        where TField : DocumentField
    {
        if (TryGetValue(key, out var raw) &&
            raw is ExtractedDocumentField<TField> typed)
        {
            value = typed;
            return true;
        }

        value = null;
        return false;
    }

    public void AddField(string key, IExtractedDocumentField value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        if (!TryAdd(key, value))
        {
            throw new InvalidOperationException(
                $"A(z) '{key}' mező már szerepel a dokumentumban.");
        }
    }

    public ExtractedDocumentFieldDictionary Clone() => new(this);
}