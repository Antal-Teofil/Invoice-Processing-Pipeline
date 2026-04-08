using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// A group of business terms providing contact information about the party.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Telephone"></param>
    /// <param name="ElectronicMail"></param>
    public sealed record Contact(string? Name, string? Telephone, string? ElectronicMail);
}
