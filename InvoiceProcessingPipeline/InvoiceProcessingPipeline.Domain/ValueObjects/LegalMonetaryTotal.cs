using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// A group of business terms providing the monetary totals for the invoice.
    /// </summary>
    public sealed record LegalMonetaryTotal(LineExtensionAmount LEAmount, 
        TaxExclusiveAmount TEAmount, 
        TaxInclusiveAmount TIAmount, 
        AllowanceTotalAmount? ATAmount, 
        ChargeTotalAmount? CTAmount, 
        PrepaidAmount? PPAmount, 
        PayableRoundingAmount? PRAmount, 
        PayableAmount? PAmount);
}
