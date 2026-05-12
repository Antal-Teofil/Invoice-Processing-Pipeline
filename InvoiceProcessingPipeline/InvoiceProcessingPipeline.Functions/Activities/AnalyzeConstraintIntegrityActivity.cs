using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Extensions;
using static InvoiceProcessingPipeline.Application.Extensions;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public sealed class AnalyzeConstraintIntegrityActivity(ILogger<AnalyzeConstraintIntegrityActivity> loigger, IDocumentDataStore store)
    {
        [Function(nameof(AnalyzeConstraintIntegrityActivity))]
        public async Task<ActivityResult<string>> RunAsync([ActivityTrigger] string documentId)
        {
            
            var document = await store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(documentId);

            if (document is null)
            {
                return ActivityResult<string>.Failure("Document Not Found");
            }

            document.Register(
                HasInvoiceNumber);
            return ActivityResult<string>.Success(documentId);
        }
    }
}
