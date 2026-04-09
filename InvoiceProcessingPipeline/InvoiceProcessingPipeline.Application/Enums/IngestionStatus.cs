using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Enums
{
    public enum IngestionStatus
    {
        APPROVED,
        UNDER_REVIEW,
        CORRECTION_REQUIRED,
        CANCELLED
    }
}
