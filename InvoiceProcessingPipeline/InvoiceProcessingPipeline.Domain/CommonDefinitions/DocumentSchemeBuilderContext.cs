
using InvoiceProcessingPipeline.Domain.ExtractionContracts;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public sealed class DocumentSchemeBuilderContext
    {
        public ExtractedDocumentData Extraction { get; private set; }

        internal DocumentSchemeBuilderContext(ExtractedDocumentData Extraction)
        {
            this.Extraction = Extraction;
        }

        public DocumentSchemeBuilder<TDocument> As<TDocument>()
            where TDocument : DocumentScheme, IDocumentScheme<TDocument>
        {
            return TDocument.Create(Extraction);
        }
    }
}
