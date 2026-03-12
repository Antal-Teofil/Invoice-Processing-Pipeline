using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.EnumTypes
{
    public enum InvoiceTypeCode : ushort
    {
        COMMERCIAL_INVOICE = 380,
        CREDIT_NOTE = 381,
        DEBIT_NOTE = 383,
        PREPAYMENT_INVOICE = 386,
        CORRECTION_INVOICE = 84,
        CANCELLATION_INVOICE = 457
    }
}
