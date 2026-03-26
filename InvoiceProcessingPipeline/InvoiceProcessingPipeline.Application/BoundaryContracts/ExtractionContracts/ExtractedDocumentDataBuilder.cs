

using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public class ExtractedDocumentDataBuilder<TSource>(TSource dataSource) where TSource : class
    {

        public TSource DataSource { get; init; } = dataSource;
        public ExtractedDocumentData DocumentData { get; init; } = new();

        public ExtractedDocumentData Build()
        {
            return DocumentData;
        }

        public ExtractedDocumentDataBuilder<TSource> ExtractField(string internalFieldName, Func<TSource, ExtractedDocumentField<DocumentField>> extractor)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(internalFieldName, nameof(internalFieldName));
            ArgumentNullException.ThrowIfNull(extractor, nameof(extractor));

            var extractedField = extractor(DataSource);

            DocumentData.FieldDictionary.Add(internalFieldName, extractedField);
            return this;
        }
    }
}
