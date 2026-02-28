using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Shared;

public sealed record ActivityError(string Code, string Message);