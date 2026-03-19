using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed record ExtractedDocumentDataSchema()
    {
        // to be refactored
        public Guid Id {  get; set; }
        public ExtractedField<AnalyzerInformation>? AnalyzerInformation { get; set; }

        public ExtractedField<Party>? AccountingSupplierParty { get; set; }

        public ExtractedField<Party>? AccountingCustomerParty { get; set; }

        public ExtractedField<IssueDate>? IssueDate { get; set; }

        public ExtractedField<DueDate>? DueDate { get; set; }

        public ExtractedField<Note>? Note {  get; set; }

        public ExtractedField<TaxPointDate>? TaxPointDate { get; set; }

        public ExtractedField<DocumentCurrencyCode>? DocumentCurrencyCode {  get; set; }

        public ExtractedField<TaxCurrencyCode>? TaxCurrencyCode { get; set; }

        public ICollection<ExtractedField<TaxTotal>>? TaxTotals { get; set; }

        public ExtractedField<LegalMonetaryTotal>? LegalMonetaryTotal { get; set; }

        public ICollection<ExtractedField<InvoiceLine>>? InvoiceLines { get; set; }

    }
}
