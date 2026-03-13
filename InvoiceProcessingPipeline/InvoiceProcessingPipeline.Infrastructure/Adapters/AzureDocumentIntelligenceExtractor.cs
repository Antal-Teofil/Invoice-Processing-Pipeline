using Azure;
using Azure.AI.DocumentIntelligence;
using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Ports;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class AzureDocumentIntelligenceExtractor(
        DocumentIntelligenceClient client) : IDocumentExtractor
    {
        private static readonly SemaphoreSlim LogLock = new(1, 1);

        // fapados megoldas, ez betolti a teljes mindenseget
        public async Task<ExtractedDocumentDataSchema> ExtractDocumentAsync(Uri sasUri, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(sasUri, nameof(sasUri));

            Operation<AnalyzeResult> result = await client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-invoice",
                sasUri,
                token);

            AnalyzeResult analyzeResult = result.Value;

            Console.WriteLine("Puszi");
            return null!;
        }
    }
}