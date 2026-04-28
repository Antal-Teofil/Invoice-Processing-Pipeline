using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.CommonDefinitions
{
    public interface IDocumentSchemeBuilder<out TDocumentScheme> where TDocumentScheme : DocumentScheme
    {
        public TDocumentScheme Build();
    }
}
