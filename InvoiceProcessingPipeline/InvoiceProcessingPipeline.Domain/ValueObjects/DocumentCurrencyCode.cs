using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record DocumentCurrencyCode()
    {
        public required string CurrencyCode { get; set; }
    }
}
