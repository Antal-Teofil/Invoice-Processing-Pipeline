using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentDataExtractor
    {
        public Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, CancellationToken token);

        
    }
}
