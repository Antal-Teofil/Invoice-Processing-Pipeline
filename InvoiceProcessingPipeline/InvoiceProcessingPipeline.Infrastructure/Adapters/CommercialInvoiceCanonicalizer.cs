using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public class CommercialInvoiceCanonicalizer : IDocumentCanonicalizer
    {
        public CommercialInvoiceDocumentScheme Canonicalize(ExtractedDocumentData exraction)
        {
            throw new NotImplementedException();
        }
    }
}
