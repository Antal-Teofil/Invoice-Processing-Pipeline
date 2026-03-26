

using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.Aggregates.Components;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(
        DocumentIntelligenceClient client) : IDocumentDataExtractor
    {

        // fapados megoldas, ez betolti a teljes mindenseget
        public async Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-invoice",
                sasUri,
                token);

            AnalyzeResult analyzedResult = result.Value;

            var extractorBuilder = new ExtractedDocumentDataBuilder<AnalyzeResult>(analyzedResult)
                .ExtractField("accounting-customer-party", (analyzedResult) =>
                {
                    return new ExtractedDocumentField<Party>();
                })
                .Build();
            return null!;
        }



    }
}