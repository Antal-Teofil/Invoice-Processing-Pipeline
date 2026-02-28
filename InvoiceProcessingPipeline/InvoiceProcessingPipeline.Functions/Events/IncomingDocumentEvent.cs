using Azure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Functions.Events
{
    public sealed class IncomingDocumentEvent(ILogger<IncomingDocumentEvent> logger)
    {
        [Function(nameof(IncomingDocumentEvent))]
        public async Task RunAsync([EventGridTrigger] CloudEvent cloudEvent, [DurableClient] DurableTaskClient durableClient) 
        {
            logger.LogInformation(
            "CloudEvent received. Id={Id}, Type={Type}, Subject={Subject}",
            cloudEvent.Id,
            cloudEvent.Type,
            cloudEvent.Subject);
        }
    }
}
