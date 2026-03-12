using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record BankAccount()
    {
        public required string Iban {  get; init; }

        public required string Bic {  get; init; }
    }
}
