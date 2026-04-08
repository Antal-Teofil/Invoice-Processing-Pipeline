using InvoiceProcessingPipeline.Domain.ValueObjects;
using System.Collections;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;

public sealed class ExtractedDocumentFieldDictionary
    : IReadOnlyDictionary<string, IExtractedDocumentField>
{
    private readonly Dictionary<string, IExtractedDocumentField> _items;

    public ExtractedDocumentFieldDictionary()
    {
        _items = new(StringComparer.OrdinalIgnoreCase);
    }

    private ExtractedDocumentFieldDictionary(Dictionary<string, IExtractedDocumentField> items)
    {
        _items = items;
    }

    public IExtractedDocumentField this[string key] => _items[key];

    public IEnumerable<string> Keys => _items.Keys;

    public IEnumerable<IExtractedDocumentField> Values => _items.Values;

    public int Count => _items.Count;

    public bool ContainsKey(string key) => _items.ContainsKey(key);

    public bool TryGetValue(string key, out IExtractedDocumentField value) =>
        _items.TryGetValue(key, out value!);

    public bool TryGet<TField>(string key, out ExtractedDocumentField<TField>? value)
        where TField : DocumentField
    {
        if (_items.TryGetValue(key, out var raw) &&
            raw is ExtractedDocumentField<TField> typed)
        {
            value = typed;
            return true;
        }

        value = null;
        return false;
    }

    internal void Add(string key, IExtractedDocumentField value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        if (!_items.TryAdd(key, value))
        {
            throw new InvalidOperationException(
                $"A(z) '{key}' mező már szerepel a dokumentumban.");
        }
    }

    internal ExtractedDocumentFieldDictionary Clone() =>
        new(new Dictionary<string, IExtractedDocumentField>(_items, StringComparer.OrdinalIgnoreCase));

    public IEnumerator<KeyValuePair<string, IExtractedDocumentField>> GetEnumerator() =>
        _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}