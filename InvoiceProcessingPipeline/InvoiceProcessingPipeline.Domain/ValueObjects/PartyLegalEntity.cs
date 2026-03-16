using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    // RegistrationName -> CustomerName/VendorName (AADI)
    //
    public sealed record PartyLegalEntity(string RegistrationName, string? CompanyID, string CompanyLegalForm);
}
