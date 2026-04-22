using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// A group of business terms providing the monetary totals for the invoice.
    /// </summary>
    public sealed record LegalMonetaryTotal(LineExtensionAmount LineExtensionAmount, 
        TaxExclusiveAmount TaxExclusiveAmount, 
        TaxInclusiveAmount TaxInclusiveAmount, 
        AllowanceTotalAmount? AllowanceTotalAmount, 
        ChargeTotalAmount? ChargeTotalAmount, 
        PrepaidAmount? PrePaidAmount, 
        PayableRoundingAmount? PayableRoundAmount, 
        PayableAmount? PayableAmount) : DocumentField;
}
