using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxSubtotal(TaxableAmount TaxableAmount, TaxAmount Amount, TaxCategory Category) : DocumentField;
}
