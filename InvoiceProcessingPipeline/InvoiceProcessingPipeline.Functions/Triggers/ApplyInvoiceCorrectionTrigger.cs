using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    public sealed class ApplyInvoiceCorrectionTrigger
    {
        [Function(nameof(ApplyInvoiceCorrectionTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "PATCH", Route = "invoices/{invoiceId}")] HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
