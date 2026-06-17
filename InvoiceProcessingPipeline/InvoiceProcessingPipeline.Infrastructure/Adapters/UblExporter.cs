using InvoiceProcessingPipeline.Application.ExportTypes;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using UblSharp;
using UblSharp.CommonAggregateComponents;
using UblSharp.UnqualifiedDataTypes;

using DomainParty = InvoiceProcessingPipeline.Domain.Aggregates.Components.Party;
using DomainInvoiceLine = InvoiceProcessingPipeline.Domain.ValueObjects.InvoiceLine;
using DomainTaxTotal = InvoiceProcessingPipeline.Domain.ValueObjects.TaxTotal;
using DomainTaxSubtotal = InvoiceProcessingPipeline.Domain.ValueObjects.TaxSubtotal;
using DomainTaxCategory = InvoiceProcessingPipeline.Domain.ValueObjects.TaxCategory;
using DomainClassifiedTaxCategory = InvoiceProcessingPipeline.Domain.ValueObjects.ClassifiedTaxCategory;
using DomainAllowanceCharge = InvoiceProcessingPipeline.Domain.ValueObjects.AllowanceCharge;
using DomainAmount = InvoiceProcessingPipeline.Domain.ValueObjects.Amount;
using DomainPrice = InvoiceProcessingPipeline.Domain.ValueObjects.Price;

namespace InvoiceProcessingPipeline.Infrastructure.Adapters
{
    public sealed class UblExporter : IDocumentSchemeExporter
    {
        private const string CustomizationId =
            "urn:cen.eu:en16931:2017#compliant#" +
            "urn:fdc:peppol.eu:2017:poacc:billing:3.0";

        private const string ProfileId =
            "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";

        private const string XmlContentType = "application/xml";
        private const string ExportFormat = "UBL";
        private const string ExportEncoding = "UTF-8";

        public async ExportedDocument Export(CommercialInvoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);

            var ublInvoice = new InvoiceType
            {
                UBLVersionID = "2.1",
                CustomizationID = CustomizationId,
                ProfileID = ProfileId,

                ID = invoice.InvoiceId!.Value,
                IssueDate = FormatDate(invoice.IssueDate!.Value),

                InvoiceTypeCode = new CodeType
                {
                    Value = "380"
                },

                DocumentCurrencyCode = new CodeType
                {
                    Value = invoice.DocumentCurrencyCode!.Value
                },

                AccountingSupplierParty = new SupplierPartyType
                {
                    Party = MapParty(invoice.AccountingSupplierParty!)
                },

                AccountingCustomerParty = new CustomerPartyType
                {
                    Party = MapParty(invoice.AccountingCustomerParty!)
                },

                TaxTotal = invoice.TaxTotals!
                    .Select(MapTaxTotal)
                    .ToList(),

                LegalMonetaryTotal =
                    MapLegalMonetaryTotal(invoice.LegalMonetaryTotal!),

                InvoiceLine = invoice.InvoiceLines!
                    .Select(MapInvoiceLine)
                    .ToList()
            };

            MapOptionalHeaderFields(invoice, ublInvoice);
            SetNamespaces(ublInvoice);

            using var stream = new MemoryStream();

            UblDocument.Save(ublInvoice, stream);

            return new ExportedDocument
            {
                FileName = $"{SanitizeFileName(invoice.InvoiceId.Value)}.xml",
                ContentType = XmlContentType,
                Format = ExportFormat,
                Encoding = ExportEncoding,
                Content = stream.ToArray()
            };
        }

        private static void MapOptionalHeaderFields(
            CommercialInvoice source,
            InvoiceType target)
        {
            if (source.DueDate is not null)
            {
                target.DueDate = FormatDate(source.DueDate.Value);
            }

            if (source.Note is not null)
            {
                target.Note = new List<TextType>
                {
                    Text(source.Note.Value)
                };
            }

            if (source.TaxPointDate is not null)
            {
                target.TaxPointDate =
                    FormatDate(source.TaxPointDate.Value);
            }

            if (source.TaxCurrencyCode is not null)
            {
                target.TaxCurrencyCode = new CodeType
                {
                    Value = source.TaxCurrencyCode.Value
                };
            }

            if (source.InvoicePeriod is not null)
            {
                target.InvoicePeriod = new List<PeriodType>
                {
                    MapInvoicePeriod(source.InvoicePeriod)
                };
            }
        }

        private static PartyType MapParty(DomainParty source)
        {
            var party = new PartyType
            {
                PostalAddress = MapPostalAddress(source.Address),

                PartyLegalEntity = new List<PartyLegalEntityType>
                {
                    MapPartyLegalEntity(source.PartyLegalEntity)
                }
            };

            if (source.Name is not null)
            {
                party.PartyName = new List<PartyNameType>
                {
                    new()
                    {
                        Name = source.Name.Value
                    }
                };
            }

            if (source.PartyTaxSchemes is not null &&
                source.PartyTaxSchemes.Count > 0)
            {
                party.PartyTaxScheme = source.PartyTaxSchemes
                    .Select(MapPartyTaxScheme)
                    .ToList();
            }

            if (source.ContactInfo is not null)
            {
                party.Contact = MapContact(source.ContactInfo);
            }

            return party;
        }

        private static AddressType MapPostalAddress(
            PostalAddress source)
        {
            var address = new AddressType
            {
                StreetName = source.StreetName,
                AdditionalStreetName = source.AdditionalStreetName,
                CityName = source.CityName,
                PostalZone = source.PostalZone,
                CountrySubentity = source.CountrySubentity,

                Country = new CountryType
                {
                    IdentificationCode = Code(
                        source.CountryIdentificationCode)
                }
            };

            if (!string.IsNullOrWhiteSpace(source.AddressLine))
            {
                address.AddressLine = new List<AddressLineType>
                {
                    new()
                    {
                        Line = source.AddressLine
                    }
                };
            }

            return address;
        }

        private static PartyLegalEntityType MapPartyLegalEntity(
            PartyLegalEntity source)
        {
            return new PartyLegalEntityType
            {
                RegistrationName = source.PartyRegistrationName,

                CompanyID = string.IsNullOrWhiteSpace(source.CompanyId)
                    ? null
                    : Identifier(source.CompanyId),

                CompanyLegalForm = source.CompanyLegalForm
            };
        }

        private static PartyTaxSchemeType MapPartyTaxScheme(
            PartyTaxScheme source)
        {
            return new PartyTaxSchemeType
            {
                CompanyID = Identifier(source.CompanyID),
                TaxScheme = MapTaxScheme(source.TaxScheme)
            };
        }

        private static ContactType MapContact(Contact source)
        {
            return new ContactType
            {
                Name = source.Name,
                Telephone = source.Telephone,
                ElectronicMail = source.ElectronicMail
            };
        }

        private static TaxTotalType MapTaxTotal(
            DomainTaxTotal source)
        {
            return new TaxTotalType
            {
                TaxAmount = MapAmount(source.Amount),

                TaxSubtotal = source.SubTotal?
                    .Select(MapTaxSubtotal)
                    .ToList()
            };
        }

        private static TaxSubtotalType MapTaxSubtotal(
            DomainTaxSubtotal source)
        {
            return new TaxSubtotalType
            {
                TaxableAmount = MapAmount(source.TaxableAmount),
                TaxAmount = MapAmount(source.Amount),
                TaxCategory = MapTaxCategory(source.Category)
            };
        }

        private static TaxCategoryType MapTaxCategory(
            DomainTaxCategory source)
        {
            var category = new TaxCategoryType
            {
                ID = Identifier(source.VatCategoryCode),

                Percent = source.Percent.HasValue
                    ? Convert.ToDecimal(
                        source.Percent.Value,
                        CultureInfo.InvariantCulture)
                    : null,

                TaxScheme = MapTaxScheme(source.TaxScheme)
            };

            if (!string.IsNullOrWhiteSpace(
                    source.TaxExemptionReasonCode))
            {
                category.TaxExemptionReasonCode =
                    Code(source.TaxExemptionReasonCode);
            }

            if (!string.IsNullOrWhiteSpace(
                    source.TaxExemptionReason))
            {
                category.TaxExemptionReason = new List<TextType>
                {
                    Text(source.TaxExemptionReason)
                };
            }

            return category;
        }

        private static TaxCategoryType MapClassifiedTaxCategory(
            DomainClassifiedTaxCategory source)
        {
            return new TaxCategoryType
            {
                ID = Identifier(source.VatCategoryCode),

                Percent = source.Percent.HasValue
                    ? Convert.ToDecimal(
                        source.Percent.Value,
                        CultureInfo.InvariantCulture)
                    : null,

                TaxScheme = MapTaxScheme(source.TaxScheme)
            };
        }

        private static TaxSchemeType MapTaxScheme(string taxScheme)
        {
            return new TaxSchemeType
            {
                ID = Identifier(taxScheme)
            };
        }

        private static MonetaryTotalType MapLegalMonetaryTotal(
            LegalMonetaryTotal source)
        {
            return new MonetaryTotalType
            {
                LineExtensionAmount =
                    MapAmount(source.LineExtensionAmount),

                TaxExclusiveAmount =
                    MapAmount(source.TaxExclusiveAmount),

                TaxInclusiveAmount =
                    MapAmount(source.TaxInclusiveAmount),

                AllowanceTotalAmount =
                    source.AllowanceTotalAmount is null
                        ? null
                        : MapAmount(source.AllowanceTotalAmount),

                ChargeTotalAmount =
                    source.ChargeTotalAmount is null
                        ? null
                        : MapAmount(source.ChargeTotalAmount),

                PrepaidAmount =
                    source.PrePaidAmount is null
                        ? null
                        : MapAmount(source.PrePaidAmount),

                PayableRoundingAmount =
                    source.PayableRoundAmount is null
                        ? null
                        : MapAmount(source.PayableRoundAmount),

                PayableAmount =
                    source.PayableAmount is null
                        ? null
                        : MapAmount(source.PayableAmount)
            };
        }

        private static InvoiceLineType MapInvoiceLine(
            DomainInvoiceLine source)
        {
            var line = new InvoiceLineType
            {
                ID = source.LineId,

                InvoicedQuantity =
                    MapQuantity(source.InvoicedQuantity),

                LineExtensionAmount =
                    MapAmount(source.LineExtensionAmount),

                Item = MapItem(source.Kind),

                Price = MapPrice(source.ItemPrice)
            };

            if (source.LineNote is not null)
            {
                line.Note = new List<TextType>
                {
                    Text(source.LineNote.Value)
                };
            }

            if (source.InvoicePeriod is not null)
            {
                line.InvoicePeriod = new List<PeriodType>
                {
                    MapInvoicePeriod(source.InvoicePeriod)
                };
            }

            if (source.AllowanceCharges is not null &&
                source.AllowanceCharges.Count > 0)
            {
                line.AllowanceCharge = source.AllowanceCharges
                    .Select(MapAllowanceCharge)
                    .ToList();
            }

            return line;
        }

        private static ItemType MapItem(Item source)
        {
            var item = new ItemType
            {
                Name = source.Name,

                ClassifiedTaxCategory = new List<TaxCategoryType>
                {
                    MapClassifiedTaxCategory(
                        source.ClassifiedTaxCategory)
                }
            };

            if (!string.IsNullOrWhiteSpace(source.Description))
            {
                item.Description = new List<TextType>
                {
                    Text(source.Description)
                };
            }

            if (!string.IsNullOrWhiteSpace(
                    source.BuyersItemIdentification))
            {
                item.BuyersItemIdentification =
                    new ItemIdentificationType
                    {
                        ID = Identifier(
                            source.BuyersItemIdentification)
                    };
            }

            if (!string.IsNullOrWhiteSpace(
                    source.SellersItemIdentification))
            {
                item.SellersItemIdentification =
                    new ItemIdentificationType
                    {
                        ID = Identifier(
                            source.SellersItemIdentification)
                    };
            }

            if (!string.IsNullOrWhiteSpace(
                    source.StandardItemIdentification))
            {
                item.StandardItemIdentification =
                    new ItemIdentificationType
                    {
                        ID = Identifier(
                            source.StandardItemIdentification)
                    };
            }

            if (!string.IsNullOrWhiteSpace(
                    source.OriginCountryCode))
            {
                item.OriginCountry = new CountryType
                {
                    IdentificationCode =
                        Code(source.OriginCountryCode)
                };
            }

            if (source.AIProperty is not null &&
                source.AIProperty.Count > 0)
            {
                item.AdditionalItemProperty = source.AIProperty
                    .Select(MapAdditionalItemProperty)
                    .ToList();
            }

            return item;
        }

        private static ItemPropertyType MapAdditionalItemProperty(
            AdditionalItemProperty source)
        {
            return new ItemPropertyType
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        private static PriceType MapPrice(DomainPrice source)
        {
            var price = new PriceType
            {
                PriceAmount = MapAmount(source.PAmount),

                BaseQuantity = source.BaseQuantity is null
                    ? null
                    : MapQuantity(source.BaseQuantity)
            };

            if (source.AllowanceCharge is not null)
            {
                price.AllowanceCharge = new List<AllowanceChargeType>
                {
                    MapAllowanceCharge(source.AllowanceCharge)
                };
            }

            return price;
        }

        private static AllowanceChargeType MapAllowanceCharge(
            DomainAllowanceCharge source)
        {
            var allowanceCharge = new AllowanceChargeType
            {
                ChargeIndicator = source.ChargeIndicator,
                Amount = MapAmount(source.Amount),

                BaseAmount = source.BaseAmount is null
                    ? null
                    : MapAmount(source.BaseAmount),

                MultiplierFactorNumeric =
                    source.MultiplierFactorNumeric.HasValue
                        ? Convert.ToDecimal(
                            source.MultiplierFactorNumeric.Value,
                            CultureInfo.InvariantCulture)
                        : null
            };

            if (!string.IsNullOrWhiteSpace(
                    source.AllowanceChargeReasonCode))
            {
                allowanceCharge.AllowanceChargeReasonCode =
                    Code(source.AllowanceChargeReasonCode);
            }

            if (!string.IsNullOrWhiteSpace(
                    source.AllowanceChargeReason))
            {
                allowanceCharge.AllowanceChargeReason =
                    new List<TextType>
                    {
                        Text(source.AllowanceChargeReason)
                    };
            }

            return allowanceCharge;
        }

        private static PeriodType MapInvoicePeriod(
            InvoicePeriod source)
        {
            var period = new PeriodType();

            if (source.StartDate.HasValue)
            {
                period.StartDate =
                    FormatDate(source.StartDate.Value);
            }

            if (source.EndDate.HasValue)
            {
                period.EndDate =
                    FormatDate(source.EndDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(
                    source.DescriptionCode))
            {
                period.DescriptionCode = new List<CodeType>
                {
                    Code(source.DescriptionCode)
                };
            }

            return period;
        }

        private static AmountType MapAmount(
            LineExtensionAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            TaxExclusiveAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            TaxInclusiveAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            AllowanceTotalAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            ChargeTotalAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            PrepaidAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            PayableRoundingAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            PayableAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            TaxableAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            TaxAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType MapAmount(
            DomainAmount source)
        {
            return CreateAmount(source.Value, source.CurrencyId);
        }

        private static AmountType MapAmount(
            BaseAmount source)
        {
            return CreateAmount(
                Convert.ToDecimal(
                    source.Value,
                    CultureInfo.InvariantCulture),
                source.CurrencyId);
        }

        private static AmountType MapAmount(
            PriceAmount source)
        {
            return CreateAmount(source.Amount, source.CurrencyId);
        }

        private static AmountType CreateAmount(
            decimal value,
            string currencyId)
        {
            return new AmountType
            {
                currencyID = currencyId,
                Value = value
            };
        }

        private static QuantityType MapQuantity(
            InvoicedQuantity source)
        {
            return new QuantityType
            {
                unitCode = source.UnitCode,
                Value = source.Quantity
            };
        }

        private static QuantityType MapQuantity(
            BaseQuantity source)
        {
            return new QuantityType
            {
                unitCode = source.UnitCode,
                Value = source.Quantity
            };
        }

        private static IdentifierType Identifier(string value)
        {
            return new IdentifierType
            {
                Value = value
            };
        }

        private static CodeType Code(string value)
        {
            return new CodeType
            {
                Value = value
            };
        }

        private static TextType Text(string value)
        {
            return new TextType
            {
                Value = value
            };
        }

        private static string FormatDate(DateOnly date)
        {
            return date.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);
        }

        private static void SetNamespaces(InvoiceType invoice)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add(
                "cac",
                "urn:oasis:names:specification:ubl:" +
                "schema:xsd:CommonAggregateComponents-2");

            namespaces.Add(
                "cbc",
                "urn:oasis:names:specification:ubl:" +
                "schema:xsd:CommonBasicComponents-2");

            namespaces.Add(
                "ext",
                "urn:oasis:names:specification:ubl:" +
                "schema:xsd:CommonExtensionComponents-2");

            invoice.Xmlns = namespaces;
        }

        private static string SanitizeFileName(string value)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();

            var result = new string(
                value.Select(character =>
                        invalidCharacters.Contains(character)
                            ? '_'
                            : character)
                    .ToArray());

            return string.IsNullOrWhiteSpace(result)
                ? "invoice"
                : result;
        }
    }
}