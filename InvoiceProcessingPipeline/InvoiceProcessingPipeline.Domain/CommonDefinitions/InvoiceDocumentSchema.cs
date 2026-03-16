using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentSchema : DocumentDataSchema
    {
        public required CustomerParty AccountingSupplierParty { get; set; }

        public required SupplierParty AccountingCustomerParty { get; set; }

    }
}
