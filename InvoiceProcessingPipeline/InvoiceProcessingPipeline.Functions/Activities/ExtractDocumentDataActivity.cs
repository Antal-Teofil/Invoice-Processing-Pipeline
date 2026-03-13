using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public class ExtractDocumentDataActivity(ILogger<ExtractDocumentDataActivity> logger, IDocumentExtractor extractor)
    {
        [Function(nameof(ExtractDocumentDataActivity))]
        public async Task<ActivityResult<ExtractedDocumentData>> RunAsync([ActivityTrigger] DocumentUserDelegationSasUri sasUri, CancellationToken token)
        {
            Uri userDelegationSasUri = sasUri.SasUri;

            if(userDelegationSasUri is null)
            {
                return ActivityResult<ExtractedDocumentData>.Failure("User Delegation SAS URI must be a non-null value");
            }

            var result = await extractor.ExtractDocumentAsync(userDelegationSasUri, token);
            return;
        }
    }
}
