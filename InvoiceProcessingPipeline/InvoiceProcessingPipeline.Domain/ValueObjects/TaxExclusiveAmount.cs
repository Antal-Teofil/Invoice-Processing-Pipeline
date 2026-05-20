using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxExclusiveAmount(decimal Amount, string CurrencyId) : DocumentField;
}
