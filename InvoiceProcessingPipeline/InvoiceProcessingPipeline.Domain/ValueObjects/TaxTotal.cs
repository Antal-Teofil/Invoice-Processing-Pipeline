using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // SubTotal -> SubTotal (AADI)
    public sealed record TaxTotal(TaxAmount Amount, TaxSubtotal? SubTotal);
}
