using InvoiceProcessingPipeline.Application.BoundaryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentExtractor
    {
        public Task<ExtractedDocumentDataSchema> ExtractDocumentAsync(Uri sasUri, CancellationToken token);
        
    }
}
