using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// identifies the Seller's electronic address to which the application level response to the invoice may be delivered
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="SchemeID"></param>
    public sealed record EndpointId(string Value, string SchemeID);
}
