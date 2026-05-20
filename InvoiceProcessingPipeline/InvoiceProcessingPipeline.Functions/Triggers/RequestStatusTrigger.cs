using Castle.Core.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InvoiceProcessingPipeline.Functions.Triggers
{
    public sealed class RequestStatusTrigger(ILogger<RequestStatusTrigger> logger)
    {
        [Function(nameof(RequestStatusTrigger))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orchestrations/{instanceId}")] HttpRequestData request)
        {
            logger.LogInformation("Requesting workflow status");
            // lekerem a folyamat allapotat vagyis a statuszt illetve a hozza tartozo szamla allapotot
            var response = request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
