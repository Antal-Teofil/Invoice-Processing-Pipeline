using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record Amount(decimal Value, string CurrencyId): DocumentField;
}
