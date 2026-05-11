using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class DocumentSchemeBuilder<TDocument>
        where TDocument : DocumentScheme
    {
        protected DocumentSchemeBuilder(ExtractedDocumentData extraction)
        {
            ArgumentNullException.ThrowIfNull(extraction);
            Extraction = extraction;
        }

        public ExtractedDocumentData Extraction { get; }

        public abstract TDocument Build();
    }
}
