using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(DocumentIntelligenceClient client, IMapper mapper) : IDocumentExtractor
    {
        public async Task<ExtractedDocumentDataSchema> ExtractDocumentAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            // nah itt tervezzuk meg hogy fog kinezni az ExtractedDocumentDataSchema
            // olvassunk doksit, good luck nigge'
            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuild-invoice", sasUri, token);

            return new ExtractedDocumentDataSchema();
        }
    }
}
