using InvoiceProcessingPipeline.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.BoundaryContracts.ExtractionContracts
{
    public sealed class ExtractedDocumentFieldDictionary : Dictionary<string, ExtractedDocumentField<DocumentField>>
    {
    }
}
  