using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentCanonicalizer
    {
        public InvoiceDocumentScheme Canonicalize(ExtractedDocumentData exraction);
    }
}
