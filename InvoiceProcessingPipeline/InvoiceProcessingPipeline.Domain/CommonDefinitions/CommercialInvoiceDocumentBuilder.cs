using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public sealed class CommercialInvoiceDocumentBuilder(ExtractedDocumentData extraction) : InvoiceDocumentSchemeBuilder<CommercialInvoice>(extraction)
    {
        public CommercialInvoice Document { get; init; } = new();
        public override CommercialInvoice Build()
        {
            return Document;
        }

        public CommercialInvoiceDocumentBuilder AddAccountingCustomerParty()
        {
            if(Extraction.TryGetField("accountingCustomerParty", out ExtractedDocumentField<Party>? field))
            {

            }
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddAccountingSupplierParty()
        {
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddInvoiceNumber()
        {
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddIssueDate()
        {
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddDueDate()
        {
            return this;
        }
        
        public CommercialInvoiceDocumentBuilder AddDocumentCurrencyCode()
        {
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddLegalMonetaryTotal()
        {
            return this;
        }

        public CommercialInvoiceDocumentBuilder AddInvoiceLine()
        {
            return this;
        }
    }
}
