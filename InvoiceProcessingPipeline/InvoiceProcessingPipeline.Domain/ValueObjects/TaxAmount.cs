using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxAmount(decimal Amount, string CurrencyId) : DocumentField;
}
