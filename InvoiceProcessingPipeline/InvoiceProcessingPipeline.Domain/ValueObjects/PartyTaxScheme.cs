using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// Party VAT/TAX identifiers
    /// </summary>
    public sealed record PartyTaxScheme(string CompanyID, string TaxScheme);
}
