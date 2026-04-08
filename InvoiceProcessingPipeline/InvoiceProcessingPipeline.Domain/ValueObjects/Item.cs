using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Domain.ValueObjects
{
    public sealed record Item(string? Description, string Name, string? BuyersItemIdentification, string? SellersItemIdentification, string? StandardItemIdentification, OriginCountry OCountry, ClassifiedTaxCategory CTCategory, ICollection<AdditionalItemProperty>? AIProperty);
}
