using InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentDataExtractor
    {
        public Task<ExtractedDocumentData> ExtractDocumentDataAsync(Uri sasUri, string processId ,CancellationToken token);

    }
}
