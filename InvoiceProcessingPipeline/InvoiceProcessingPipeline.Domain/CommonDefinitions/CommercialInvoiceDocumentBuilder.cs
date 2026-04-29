using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public sealed class CommercialInvoiceDocumentBuilder(ExtractedDocumentData extraction) : InvoiceDocumentSchemeBuilder<CommercialInvoice>(extraction)
    {
        public override CommercialInvoice Build()
        {
            return new CommercialInvoice();
        }
    }
}
