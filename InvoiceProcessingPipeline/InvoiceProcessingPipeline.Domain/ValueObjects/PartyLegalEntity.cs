using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // RegistrationName -> CustomerName/VendorName (AADI)
    //
    /// <summary>
    /// the legal representation of a party
    /// </summary>
    /// <param name="RegistrationName"></param>
    /// <param name="CompanyID"></param>
    /// <param name="CompanyLegalForm"></param>
    public sealed record PartyLegalEntity(string PartyRegistrationName, string? CompanyId, string CompanyLegalForm) : DocumentField;
}
