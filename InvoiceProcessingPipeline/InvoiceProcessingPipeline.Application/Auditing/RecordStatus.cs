using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Auditing
{
    public sealed record RecordStatus(bool Exists) { }
}
