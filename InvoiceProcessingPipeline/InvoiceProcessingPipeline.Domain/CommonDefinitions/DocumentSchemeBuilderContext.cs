using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;

public sealed class DocumentSchemeBuilderContext
{
    public ExtractedDocumentData Extraction { get; }

    internal DocumentSchemeBuilderContext(ExtractedDocumentData extraction)
    {
        ArgumentNullException.ThrowIfNull(extraction);
        Extraction = extraction;
    }

    public DocumentSchemeBuilder<TDocument> As<TDocument>()
        where TDocument : DocumentScheme, IDocumentScheme<TDocument>
    {
        return TDocument.Create(Extraction);
    }
}