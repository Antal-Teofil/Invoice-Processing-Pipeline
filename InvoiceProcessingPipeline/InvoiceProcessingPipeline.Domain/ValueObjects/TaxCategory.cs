using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record TaxCategory(string VatCategoryCode, float? Percent, string? TaxExemptionReasonCode, string? TaxExemptionReason, string TaxScheme) : DocumentField;
}
