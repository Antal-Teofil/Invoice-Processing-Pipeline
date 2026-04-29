using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentSchemeBuilder<TDocument>(ExtractedDocumentData extraction) where TDocument : DocumentScheme
    {
        public ExtractedDocumentData Extraction { get; protected set; } = extraction;

        public abstract TDocument Build();
    }
}
