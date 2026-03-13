using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentSchema : DocumentSchema
    {
        public required Party Vendor { get; set; }

        public required Party Client { get; set; }


    }
}
