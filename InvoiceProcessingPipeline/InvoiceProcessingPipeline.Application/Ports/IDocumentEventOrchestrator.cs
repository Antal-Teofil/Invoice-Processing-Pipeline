using InvoiceProcessingPipeline.Application.BoundaryContracts;
using InvoiceProcessingPipeline.Application.Shared;
using Microsoft.DurableTask.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Ports
{
    public interface IDocumentEventOrchestrator
    {
        public Task OrchestrateEventAsync(DurableTaskClient client, string orchestratorName, DocumentIngestionEvent docEvent, CancellationToken token);
    }
}
