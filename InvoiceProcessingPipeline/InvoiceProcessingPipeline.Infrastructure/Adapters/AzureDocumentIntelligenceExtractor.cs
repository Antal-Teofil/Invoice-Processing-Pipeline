using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(DocumentIntelligenceClient client) : IDocumentExtractor
    {
        public async Task<ExtractedDocumentData> ExtractDocumentAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));
            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuild-invoice", sasUri, token);

            return null;
        }
    }
}
