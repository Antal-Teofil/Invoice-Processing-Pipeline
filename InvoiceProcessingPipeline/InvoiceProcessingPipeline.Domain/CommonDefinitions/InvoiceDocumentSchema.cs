
using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.EnumTypes;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public abstract class InvoiceDocumentSchema : DocumentDataSchema
    {
        // 1..1
        public required InvoiceId Id { get; set; }
        // 1..1
        /// <summary>
        /// a group of business terms providing information about the SELLER.
        /// </summary>
        public required Party AccountingSupplierParty { get; set; }

        // 1..1
        /// <summary>
        /// a group of business terms providing information about the BUYER.
        /// </summary>
        public required Party AccountingCustomerParty { get; set; }

        // 1..1
        /// <summary>
        /// the date when the invoice was issued.
        /// </summary>
        public required IssueDate IssueDate { get; set; }

        // 0..1
        /// <summary>
        /// payment due date.
        /// </summary>
        public DueDate? DueDate { get; set; }

        // 1..1
        public required InvoiceTypeCode InvoiceTypeCode { get; set; }

        // 0..1
        public Note? Note { get; set; }

        // 0..1
        public TaxPointDate? TaxPointDate { get; set; }

        // 1.1
        /// <summary>
        /// the currency in which all invoice amount are given, except fot the Total VAT amount in accounting currency.
        /// </summary>
        public required DocumentCurrencyCode DocumentCurrencyCode { get; set; }

        // 0..1
        public TaxCurrencyCode? TaxCurrencyCode { get; set; }

        // 1..2
        public required ICollection<TaxTotal> TaxTotals { get; set; }

        // 1..1
        public required LegalMonetaryTotal LegalMonetaryTotal { get; set; }

        // 1..n
        public required ICollection<InvoiceLine> InvoiceLines {  get; set; }
    }
}
