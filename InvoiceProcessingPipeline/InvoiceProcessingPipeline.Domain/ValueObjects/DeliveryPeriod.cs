using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record DeliveryPeriod()
    {
        public required DateOnly? StartDate { get; init; }
        public required DateOnly? EndDate { get; init; }
    }
}
