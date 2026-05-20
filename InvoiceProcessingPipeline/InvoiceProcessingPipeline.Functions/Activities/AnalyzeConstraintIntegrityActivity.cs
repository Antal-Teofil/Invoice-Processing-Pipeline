using InvoiceProcessingPipeline.Application.Extensions;
using static InvoiceProcessingPipeline.Application.Extensions.DocumentSchemeExtensions;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Application.Shared;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public sealed class AnalyzeConstraintIntegrityActivity(ILogger<AnalyzeConstraintIntegrityActivity> logger, IDocumentDataStore documentStore, IDocumentAuditStore auditStore)
    {
        [Function(nameof(AnalyzeConstraintIntegrityActivity))]
        public async Task<ActivityResult<bool>> RunAsync([ActivityTrigger] string documentId)
        {
            
            var document = await documentStore.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(documentId);

            if (document is null)
            {
                return ActivityResult<bool>.Failure("Document Not Found");
            }

            var result = document.Register(
                HasInvoiceNumber,
                HasCustomerParty,
                HasSupplierParty,
                HasIssueDate,
                HasDocumentCurrencyCode,
                HasInvoiceLines,
                HasLegalMonetaryTotal
                );


            var recordId = await auditStore.StoreIssueRecord(result, document.DocumentId.ToString());

            if(result.Issues.Count == 0)
            {
                return ActivityResult<bool>.Success(false);
            }
            return ActivityResult<bool>.Failure("Some issue has happened!");
        }
    }
}
