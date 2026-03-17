using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public class AzureDocumentIntelligenceExtractedSchemaAdapter : IExtractionResultAdapter<AnalyzeResult>
    {

        public Task<ExtractedDocumentDataSchema> AdaptSchema(AnalyzeResult source)
        {
            throw new NotImplementedException();
        }
    }
}
