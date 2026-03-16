using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record Price(PriceAmount PAmount, BaseQuantity? BQuantity, AllowanceCharge? ACharge);
}
