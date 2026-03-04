using InvoiceProcessingPipeline.Application.BoundaryContracts;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Functions.Activities
{
    public class ExtractDocumentDataActivity
    {
        [Function(nameof(ExtractDocumentDataActivity))]
        public async Task RunAsync([ActivityTrigger] DocumentStorageMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}
