using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxSubtotal(TaxableAmount TbleAmount, TaxAmount Amount, TaxCategory Category);
}
