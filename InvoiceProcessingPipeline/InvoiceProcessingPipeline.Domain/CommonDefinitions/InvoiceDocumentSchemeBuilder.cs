using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentSchemeBuilder<TDocument>(ExtractedDocumentData extraction)
        : DocumentSchemeBuilder<TDocument>(extraction)
        where TDocument: InvoiceDocumentScheme
    {
    }
}
