using InvoiceProcessingPipeline.Application.Validations;
using InvoiceProcessingPipeline.Domain.Aggregates.Components;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.CommonDefinitions;
using InvoiceProcessingPipeline.Domain.ValueObjects;

namespace InvoiceProcessingPipeline.Application.Extensions
{
    public static class DocumentSchemeExtensions
    {
        public delegate IReadOnlyList<Issue> Validator<TDocument>(TDocument document)
        where TDocument : DocumentScheme;

        public static Accumulator Register<TDocument>(
            this TDocument document,
            params Validator<TDocument>[] validators)
            where TDocument : DocumentScheme
        {
            Accumulator accumulator = new();

            foreach (var validator in validators)
            {
                var issues = validator(document);
                accumulator.Append(issues);
            }
            return accumulator;
        }

        public static Issue[] HasInvoiceNumber(CommercialInvoice invoice)
        {
            if (string.IsNullOrWhiteSpace(invoice!.InvoiceId?.Value))
            {
                return [
                    new Issue {
                    FieldName = "invoiceNumber",
                    Message = "An invoice shall have an invoice number!",
                    Code = new("BR-02"),
                    Severity = Severity.ERROR,
                    }];
            }
            return [];
        }

        public static Issue[] HasCustomerParty(CommercialInvoice invoice)
        {
            var issues = new List<Issue>();

            var accountingCustomerParty = invoice.AccountingCustomerParty;

            if (accountingCustomerParty is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "accountingCustomerParty",
                    Message = "An invoice shall have a customer party!",
                    Code = new("BR-03"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            issues.AddRange(HasValidAddress(accountingCustomerParty));
            issues.AddRange(HasPartyLegalEntity(accountingCustomerParty));

            return [.. issues];
        }

        public static Issue[] HasSupplierParty(CommercialInvoice invoice)
        {
            var issues = new List<Issue>();

            var accountingSupplierParty = invoice.AccountingSupplierParty;

            if (accountingSupplierParty is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "accountingSupplierParty",
                    Message = "An invoice shall have a supplier party!",
                    Code = new("BR-04"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            issues.AddRange(HasValidAddress(accountingSupplierParty));
            issues.AddRange(HasPartyLegalEntity(accountingSupplierParty));

            return [.. issues];
        }
        private static Issue[] HasValidAddress(Party party)
        {
            var issues = new List<Issue>();

            var address = party.Address;

            if (address is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "partyPostalAddress",
                    Message = "A party shall have an address!",
                    Code = new("BR-05"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (string.IsNullOrWhiteSpace(address.StreetName))
            {
                issues.Add(new Issue
                {
                    FieldName = "partyPostalAddress",
                    Message = "A party's address shall have a street name!",
                    Code = new("BR-06"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(address.CityName))
            {
                issues.Add(new Issue
                {
                    FieldName = "partyPostalAddress",
                    Message = "A party's address shall have a city name!",
                    Code = new("BR-07"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(address.PostalZone))
            {
                issues.Add(new Issue
                {
                    FieldName = "partyPostalAddress",
                    Message = "A party's address shall have a postal zone!",
                    Code = new("BR-08"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        private static Issue[] HasPartyLegalEntity(Party party)
        {
            var issues = new List<Issue>();

            var partyLegalEntity = party.PartyLegalEntity;

            if (partyLegalEntity is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "partyLegalEntity",
                    Message = "A party shall have a legal entity!",
                    Code = new("BR-09"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (string.IsNullOrWhiteSpace(partyLegalEntity.PartyRegistrationName))
            {
                issues.Add(new Issue
                {
                    FieldName = "partyLegalEntity.registrationName",
                    Message = "A party legal entity shall have a registration name!",
                    Code = new("BR-10"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        public static Issue[] HasIssueDate(CommercialInvoice invoice)
        {
            if (invoice.IssueDate is null)
            {
                return [
                    new Issue {
                    FieldName = "issueDate",
                    Message = "An invoice shall have an issue date!",
                    Code = new("BR-11"),
                    Severity = Severity.ERROR,
                    }];
            }
            return [];
        }

        public static Issue[] HasDocumentCurrencyCode(CommercialInvoice invoice)
        {
            if (invoice.DocumentCurrencyCode is null)
            {
                return [
                    new Issue {
                    FieldName = "documentCurrencyCode",
                    Message = "An invoice shall have a document currency code!",
                    Code = new("BR-12"),
                    Severity = Severity.ERROR,
                    }];
            }
            return [];
        }

        public static Issue[] HasInvoiceLines(CommercialInvoice invoice)
        {
            var issues = new List<Issue>();

            var invoiceLines = invoice.InvoiceLines;

            if (invoiceLines is null || invoiceLines.Count == 0)
            {
                return [new Issue
        {
            FieldName = "invoiceLines",
            Message = "An invoice shall have at least one invoice line!",
            Code = new("BR-13"),
            Severity = Severity.ERROR,
        }];
            }

            foreach (var line in invoiceLines)
            {
                if (string.IsNullOrWhiteSpace(line.LineId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "invoiceLine.id",
                        Message = "An invoice line shall have a line id!",
                        Code = new("BR-13"),
                        Severity = Severity.ERROR,
                    });
                }

                issues.AddRange(HasInvoicedQuantity(line));
                issues.AddRange(HasLineExtensionAmount(line));
                issues.AddRange(HasItem(line));
                issues.AddRange(HasPrice(line));
            }

            return [.. issues];
        }

        private static Issue[] HasInvoicedQuantity(InvoiceLine line)
        {
            var issues = new List<Issue>();

            var invoicedQuantity = line.InvoicedQuantity;

            if (invoicedQuantity is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "invoicedQuantity",
                    Message = "An invoice line shall have an invoiced quantity!",
                    Code = new("BR-14"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (invoicedQuantity.Quantity <= 0)
            {
                issues.Add(new Issue
                {
                    FieldName = "invoicedQuantity",
                    Message = "An invoice line invoiced quantity shall be greater than zero!",
                    Code = new("BR-15"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(invoicedQuantity.UnitCode))
            {
                issues.Add(new Issue
                {
                    FieldName = "invoicedQuantity.unitCode",
                    Message = "An invoice line invoiced quantity shall have a unit code!",
                    Code = new("BR-16"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        private static Issue[] HasLineExtensionAmount(InvoiceLine line)
        {
            var issues = new List<Issue>();

            var lineExtensionAmount = line.LineExtensionAmount;

            if (lineExtensionAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "lineExtensionAmount",
                    Message = "An invoice line shall have a line extension amount!",
                    Code = new("BR-17"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (lineExtensionAmount.Amount < 0)
            {
                issues.Add(new Issue
                {
                    FieldName = "lineExtensionAmount.amount",
                    Message = "An invoice line extension amount shall not be negative!",
                    Code = new("BR-18"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(lineExtensionAmount.CurrencyId))
            {
                issues.Add(new Issue
                {
                    FieldName = "lineExtensionAmount.currencyId",
                    Message = "An invoice line extension amount shall have a currency ID!",
                    Code = new("BR-19"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        private static Issue[] HasItem(InvoiceLine line)
        {
            var issues = new List<Issue>();

            var item = line.Kind;

            if (item is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "item",
                    Message = "An invoice line shall have an item!",
                    Code = new("BR-20"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                issues.Add(new Issue
                {
                    FieldName = "item.name",
                    Message = "An invoice line item shall have a name!",
                    Code = new("BR-21"),
                    Severity = Severity.ERROR,
                });
            }

            if (!string.IsNullOrWhiteSpace(item.OriginCountryCode) && item.OriginCountryCode.Length != 2)
            {
                issues.Add(new Issue
                {
                    FieldName = "item.originCountryCode",
                    Message = "An invoice line item origin country code shall be a valid two-letter country code!",
                    Code = new("BR-23"),
                    Severity = Severity.ERROR,
                });
            }

            issues.AddRange(HasClassifiedTaxCategory(item.ClassifiedTaxCategory));

            return [.. issues];
        }

        private static Issue[] HasClassifiedTaxCategory(ClassifiedTaxCategory? classifiedTaxCategory)
        {
            var issues = new List<Issue>();

            if (classifiedTaxCategory is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "classifiedTaxCategory",
                    Message = "An invoice line item shall have a classified tax category!",
                    Code = new("BR-22"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (string.IsNullOrWhiteSpace(classifiedTaxCategory.VatCategoryCode))
            {
                issues.Add(new Issue
                {
                    FieldName = "classifiedTaxCategory.vatCategoryCode",
                    Message = "A classified tax category shall have a VAT category code!",
                    Code = new("BR-24"),
                    Severity = Severity.ERROR,
                });
            }

            if (classifiedTaxCategory.Percent.HasValue &&
                (classifiedTaxCategory.Percent.Value < 0 || classifiedTaxCategory.Percent.Value > 100))
            {
                issues.Add(new Issue
                {
                    FieldName = "classifiedTaxCategory.percent",
                    Message = "A classified tax category percent shall be between 0 and 100!",
                    Code = new("BR-25"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(classifiedTaxCategory.TaxScheme))
            {
                issues.Add(new Issue
                {
                    FieldName = "classifiedTaxCategory.taxScheme",
                    Message = "A classified tax category shall have a tax scheme!",
                    Code = new("BR-26"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        private static Issue[] HasPrice(InvoiceLine line)
        {
            var issues = new List<Issue>();

            var price = line.ItemPrice;

            if (price is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "price",
                    Message = "An invoice line shall have an item price!",
                    Code = new("BR-27"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            issues.AddRange(HasPriceAmount(price.PAmount));
            issues.AddRange(HasBaseQuantity(price.BaseQuantity));

            return [.. issues];
        }

        private static Issue[] HasPriceAmount(PriceAmount? priceAmount)
        {
            var issues = new List<Issue>();

            if (priceAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "price.priceAmount",
                    Message = "An invoice line price shall have a price amount!",
                    Code = new("BR-28"),
                    Severity = Severity.ERROR,
                });

                return [.. issues];
            }

            if (priceAmount.Amount < 0)
            {
                issues.Add(new Issue
                {
                    FieldName = "price.priceAmount.amount",
                    Message = "An invoice line price amount shall not be negative!",
                    Code = new("BR-29"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(priceAmount.CurrencyId))
            {
                issues.Add(new Issue
                {
                    FieldName = "price.priceAmount.currencyId",
                    Message = "An invoice line price amount shall have a currency ID!",
                    Code = new("BR-30"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        private static Issue[] HasBaseQuantity(BaseQuantity? baseQuantity)
        {
            var issues = new List<Issue>();

            if (baseQuantity is null)
            {
                return [.. issues];
            }

            if (baseQuantity.Quantity == 0)
            {
                issues.Add(new Issue
                {
                    FieldName = "price.baseQuantity.quantity",
                    Message = "A price base quantity shall be greater than zero!",
                    Code = new("BR-31"),
                    Severity = Severity.ERROR,
                });
            }

            if (string.IsNullOrWhiteSpace(baseQuantity.UnitCode))
            {
                issues.Add(new Issue
                {
                    FieldName = "price.baseQuantity.unitCode",
                    Message = "A price base quantity shall have a unit code!",
                    Code = new("BR-32"),
                    Severity = Severity.ERROR,
                });
            }

            return [.. issues];
        }

        public static Issue[] HasLegalMonetaryTotal(CommercialInvoice invoice)
        {
            var issues = new List<Issue>();

            var legalMonetaryTotal = invoice.LegalMonetaryTotal;

            if (legalMonetaryTotal is null)
            {
                return [
                    new Issue
            {
                FieldName = "legalMonetaryTotal",
                Message = "An invoice shall have a legal monetary total!",
                Code = new("BR-33"),
                Severity = Severity.ERROR,
            }
                ];
            }

            if (legalMonetaryTotal.LineExtensionAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "legalMonetaryTotal.lineExtensionAmount",
                    Message = "A legal monetary total shall have a line extension amount!",
                    Code = new("BR-34"),
                    Severity = Severity.ERROR,
                });
            }
            else
            {
                if (legalMonetaryTotal.LineExtensionAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.lineExtensionAmount.amount",
                        Message = "Line extension amount shall not be negative!",
                        Code = new("BR-35"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.LineExtensionAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.lineExtensionAmount.currencyId",
                        Message = "Line extension amount shall have a currency ID!",
                        Code = new("BR-36"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.TaxExclusiveAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "legalMonetaryTotal.taxExclusiveAmount",
                    Message = "A legal monetary total shall have a tax exclusive amount!",
                    Code = new("BR-37"),
                    Severity = Severity.ERROR,
                });
            }
            else
            {
                if (legalMonetaryTotal.TaxExclusiveAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxExclusiveAmount.amount",
                        Message = "Tax exclusive amount shall not be negative!",
                        Code = new("BR-38"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.TaxExclusiveAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxExclusiveAmount.currencyId",
                        Message = "Tax exclusive amount shall have a currency ID!",
                        Code = new("BR-39"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.TaxInclusiveAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "legalMonetaryTotal.taxInclusiveAmount",
                    Message = "A legal monetary total shall have a tax inclusive amount!",
                    Code = new("BR-40"),
                    Severity = Severity.ERROR,
                });
            }
            else
            {
                if (legalMonetaryTotal.TaxInclusiveAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxInclusiveAmount.amount",
                        Message = "Tax inclusive amount shall not be negative!",
                        Code = new("BR-41"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.TaxInclusiveAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxInclusiveAmount.currencyId",
                        Message = "Tax inclusive amount shall have a currency ID!",
                        Code = new("BR-42"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.AllowanceTotalAmount is not null)
            {
                if (legalMonetaryTotal.AllowanceTotalAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.allowanceTotalAmount.amount",
                        Message = "Allowance total amount shall not be negative!",
                        Code = new("BR-43"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.AllowanceTotalAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.allowanceTotalAmount.currencyId",
                        Message = "Allowance total amount shall have a currency ID!",
                        Code = new("BR-44"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.ChargeTotalAmount is not null)
            {
                if (legalMonetaryTotal.ChargeTotalAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.chargeTotalAmount.amount",
                        Message = "Charge total amount shall not be negative!",
                        Code = new("BR-45"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.ChargeTotalAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.chargeTotalAmount.currencyId",
                        Message = "Charge total amount shall have a currency ID!",
                        Code = new("BR-46"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.PrePaidAmount is not null)
            {
                if (legalMonetaryTotal.PrePaidAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.prepaidAmount.amount",
                        Message = "Prepaid amount shall not be negative!",
                        Code = new("BR-47"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.PrePaidAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.prepaidAmount.currencyId",
                        Message = "Prepaid amount shall have a currency ID!",
                        Code = new("BR-48"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.PayableRoundAmount is not null)
            {
                // A kerekítés lehet negatív is, ezért itt nincs Amount < 0 ellenőrzés.

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.PayableRoundAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.payableRoundingAmount.currencyId",
                        Message = "Payable rounding amount shall have a currency ID!",
                        Code = new("BR-49"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.PayableAmount is null)
            {
                issues.Add(new Issue
                {
                    FieldName = "legalMonetaryTotal.payableAmount",
                    Message = "A legal monetary total shall have a payable amount!",
                    Code = new("BR-50"),
                    Severity = Severity.ERROR,
                });
            }
            else
            {
                if (legalMonetaryTotal.PayableAmount.Amount < 0)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.payableAmount.amount",
                        Message = "Payable amount shall not be negative!",
                        Code = new("BR-51"),
                        Severity = Severity.ERROR,
                    });
                }

                if (string.IsNullOrWhiteSpace(legalMonetaryTotal.PayableAmount.CurrencyId))
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.payableAmount.currencyId",
                        Message = "Payable amount shall have a currency ID!",
                        Code = new("BR-52"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            const decimal tolerance = 0.01m;

            if (invoice.InvoiceLines is not null &&
                legalMonetaryTotal.LineExtensionAmount is not null)
            {
                var invoiceLinesTotal = invoice.InvoiceLines
                    .Where(line => line.LineExtensionAmount is not null)
                    .Sum(line => line.LineExtensionAmount!.Amount);

                if (Math.Abs(legalMonetaryTotal.LineExtensionAmount.Amount - invoiceLinesTotal) > tolerance)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.lineExtensionAmount",
                        Message = "Line extension amount shall equal the sum of invoice line extension amounts!",
                        Code = new("BR-53"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.LineExtensionAmount is not null &&
                legalMonetaryTotal.TaxExclusiveAmount is not null)
            {
                var expectedTaxExclusiveAmount =
                    legalMonetaryTotal.LineExtensionAmount.Amount
                    - (legalMonetaryTotal.AllowanceTotalAmount?.Amount ?? 0m)
                    + (legalMonetaryTotal.ChargeTotalAmount?.Amount ?? 0m);

                if (Math.Abs(legalMonetaryTotal.TaxExclusiveAmount.Amount - expectedTaxExclusiveAmount) > tolerance)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxExclusiveAmount",
                        Message = "Tax exclusive amount shall equal line extension amount minus allowance total amount plus charge total amount!",
                        Code = new("BR-54"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.TaxExclusiveAmount is not null &&
                legalMonetaryTotal.TaxInclusiveAmount is not null)
            {
                if (legalMonetaryTotal.TaxInclusiveAmount.Amount < legalMonetaryTotal.TaxExclusiveAmount.Amount)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.taxInclusiveAmount",
                        Message = "Tax inclusive amount shall not be less than tax exclusive amount!",
                        Code = new("BR-55"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            if (legalMonetaryTotal.TaxInclusiveAmount is not null &&
                legalMonetaryTotal.PayableAmount is not null)
            {
                var expectedPayableAmount =
                    legalMonetaryTotal.TaxInclusiveAmount.Amount
                    - (legalMonetaryTotal.PrePaidAmount?.Amount ?? 0m)
                    + (legalMonetaryTotal.PayableRoundAmount?.Amount ?? 0m);

                if (Math.Abs(legalMonetaryTotal.PayableAmount.Amount - expectedPayableAmount) > tolerance)
                {
                    issues.Add(new Issue
                    {
                        FieldName = "legalMonetaryTotal.payableAmount",
                        Message = "Payable amount shall equal tax inclusive amount minus prepaid amount plus payable rounding amount!",
                        Code = new("BR-56"),
                        Severity = Severity.ERROR,
                    });
                }
            }

            return [.. issues];
        }
    }
}
