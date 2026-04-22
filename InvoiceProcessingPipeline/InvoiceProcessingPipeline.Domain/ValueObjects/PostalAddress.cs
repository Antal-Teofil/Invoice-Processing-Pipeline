using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    /// <summary>
    /// Party's postal address
    /// </summary>
    /// // Vendor-/CustomerAddress
    public sealed record PostalAddress(string? StreetName, string? AdditionalStreetName, string? CityName, string? PostalZone, string? CountrySubentity, string? AddressLine, string CountryIdentificationCode) : DocumentField;
}
