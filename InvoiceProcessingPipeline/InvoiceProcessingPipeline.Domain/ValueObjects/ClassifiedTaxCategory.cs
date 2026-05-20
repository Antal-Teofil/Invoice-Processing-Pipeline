using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record ClassifiedTaxCategory(string VatCategoryCode, float? Percent, string TaxScheme) : DocumentField;
}
