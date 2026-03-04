using Azure;
using Azure.AI.DocumentIntelligence;
using Castle.Core.Logging;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public class ExtractDocumentDataActivity(ILogger<ExtractDocumentDataActivity> logger, CosmosClient cosmosClient, DocumentIntelligenceClient documentIntelligenceClient)
    {
        [Function(nameof(ExtractDocumentDataActivity))]
        public async Task RunAsync([ActivityTrigger] DocumentSasUri metadata, CancellationToken token)
        {
            Uri SasUri = metadata.SasUri;

            Operation<AnalyzeResult> result = await documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", SasUri, token);
            AnalyzedDocument analyzedDoc = result.Value.Documents.First();
            Console.WriteLine(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            return;
        }
    }
}
