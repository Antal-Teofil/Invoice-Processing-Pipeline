using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.EnumTypes;
using InvoiceProcessingPipeline.Domain.ExtractionContracts;
using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes
{
    public sealed class CommercialInvoice : InvoiceDocumentScheme, IDocumentScheme<CommercialInvoice>
    {

        // 1..1
        /// <summary>
        /// a unique identification of the Invoice
        /// </summary>
        public InvoiceNumber? InvoiceId { get; set; }
        // 1..1
        /// <summary>
        /// a group of business terms providing information about the SELLER.
        /// </summary>
        public Party? AccountingSupplierParty { get; set; }

        // 1..1
        /// <summary>
        /// a group of business terms providing information about the BUYER.
        /// </summary>
        public Party? AccountingCustomerParty { get; set; }

        // 1..1
        /// <summary>
        /// the date when the invoice was issued.
        /// </summary>
        public IssueDate? IssueDate { get; set; }

        // 0..1
        /// <summary>
        /// payment due date.
        /// </summary>
        public DueDate? DueDate { get; set; }

        // 0..1
        public Note? Note { get; set; }

        // 0..1
        public TaxPointDate? TaxPointDate { get; set; }

        // 1.1
        /// <summary>
        /// the currency in which all invoice amount are given, except fot the Total VAT amount in accounting currency.
        /// </summary>
        public DocumentCurrencyCode? DocumentCurrencyCode { get; set; }

        // 0..1
        public TaxCurrencyCode? TaxCurrencyCode { get; set; }

        public InvoicePeriod? InvoicePeriod { get; set; }
        // 1..2
        public ICollection<TaxTotal>? TaxTotals { get; set; }

        // 1..1
        public LegalMonetaryTotal? LegalMonetaryTotal { get; set; }

        // 1..n
        public ICollection<InvoiceLine>? InvoiceLines { get; set; }
        public override InvoiceTypeCode TypeCode { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public static DocumentSchemeBuilder<CommercialInvoice> Create(ExtractedDocumentData extraction)
        {
            return new CommercialInvoiceDocumentBuilder(extraction);
        }
    }
}
