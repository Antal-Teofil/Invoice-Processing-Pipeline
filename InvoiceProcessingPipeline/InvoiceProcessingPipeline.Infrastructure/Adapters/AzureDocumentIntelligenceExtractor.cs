using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Application.Ports;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(
        DocumentIntelligenceClient client, IExtractionResultAdapter<AnalyzeResult> extractionAdapter) : IDocumentExtractor
    {

        // fapados megoldas, ez betolti a teljes mindenseget
        public async Task<ExtractedDocumentResponse> ExtractDocumentDataAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-invoice",
                sasUri,
                token);

            AnalyzeResult analyzedResult = result.Value;

            ExtractedDocumentDataSchema schema =  await extractionAdapter.AdaptSchema(analyzedResult);
            return null!;
        }

    }
}