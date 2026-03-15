using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IExtractionResultAdapter<in TSource>
    {
        Task<ExtractedDocumentDataSchema> AdaptSchema(TSource source);
    }
}
