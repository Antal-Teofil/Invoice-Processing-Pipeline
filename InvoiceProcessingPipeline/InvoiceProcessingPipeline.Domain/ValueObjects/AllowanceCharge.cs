using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record AllowanceCharge(bool ChargeIndicator, Amount Amount, BaseAmount? BaseAmount, string? AllowanceChargeReasonCode, string? AllowanceChargeReason, float? MultiplierFactorNumeric);
}
