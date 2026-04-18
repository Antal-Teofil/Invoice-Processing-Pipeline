using Castle.Core.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    public sealed class RetrieveExtractedDocumentData(ILogger<RetrieveExtractedDocumentData> logger)
    {
        [Function(nameof(RetrieveExtractedDocumentData))]
        public async Task RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "verify/{invoiceId}")] HttpRequestData req)
        {

        }
    }
}
