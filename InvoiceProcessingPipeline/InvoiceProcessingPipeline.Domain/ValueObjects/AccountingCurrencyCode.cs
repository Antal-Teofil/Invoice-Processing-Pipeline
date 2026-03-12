using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record AccountingCurrencyCode()
    {
        public required string CurrecnyCode {  get; init; }
    }
}
