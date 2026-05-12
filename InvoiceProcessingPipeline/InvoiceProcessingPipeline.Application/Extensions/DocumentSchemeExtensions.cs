using InvoiceProcessingPipeline.Application.Validations;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;

namespace InvoiceProcessingPipeline.Application.Extensions
{
    public static class DocumentSchemeExtensions
    {
        public delegate IReadOnlyList<Issue> Validator<TDocument>(TDocument document)
        where TDocument : DocumentScheme;

        public static Accumulator Register<TDocument>(
            this TDocument document,
            params Validator<TDocument>[] validators)
            where TDocument : DocumentScheme
        {
            Accumulator accumulator = new();

            foreach(var validator in validators)
            {
                var issues = validator(document);
                accumulator.Append(issues);
            }
            return accumulator;
        }

        public static Issue[] HasInvoiceNumber(CommercialInvoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice!.InvoiceId?.value))
            {
                return [
                    new Issue {
                    FieldName = "invoiceNumber",
                    Message = "An invoice shall have an invoice number!",
                    Code = new("BR-02"),
                    Severity = Severity.ERROR,
                    }];
            }
            return [];
        }
    }
}
