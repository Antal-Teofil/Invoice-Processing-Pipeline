using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// An identification of the specification containing the total set of rules regarding semantic content,
    /// cardinalities and business rules to which the data contained in the instance document conforms.
    /// </summary>
    public sealed record CustomizationId();
}
