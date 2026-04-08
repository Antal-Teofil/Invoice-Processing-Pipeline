using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    // retrieves invoices under processing
    public sealed class RetrievePagedInvoicesTrigger
    {
        [Function(nameof(RetrievePagedInvoicesTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "invoices")] HttpRequestData request)
        {
            return request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
    }
}
