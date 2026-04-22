using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxInclusiveAmount(decimal Amount, string CurrencyId): DocumentField;
}
