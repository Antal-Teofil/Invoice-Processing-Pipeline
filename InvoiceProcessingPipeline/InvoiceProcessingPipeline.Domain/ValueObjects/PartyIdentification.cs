using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    //1..1
    /// <summary>
    /// Party Identification
    /// </summary>
    /// <param name="ID"></param>
    public sealed record PartyIdentification(string Value, string? SchemeID);
}
