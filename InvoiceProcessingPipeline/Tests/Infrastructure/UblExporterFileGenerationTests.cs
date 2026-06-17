using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Domain.ValueObjects;
using InvoiceProcessingPipeline.Infrastructure.Adapters;
using Microsoft.VisualBasic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using UblSharp.Validation;
using DomainParty =
    InvoiceProcessingPipeline.Domain.Aggregates.Components.Party;
using DueDate = InvoiceProcessingPipeline.Domain.ValueObjects.DueDate;
namespace InvoiceProcessingPipeline.Tests.Infrastructure.Adapters
{
    public sealed class UblExporterFileGenerationTests
    {
        private readonly ITestOutputHelper _output;

        public UblExporterFileGenerationTests(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Export_ShouldGenerateInvoiceXmlFile()
        {
            var invoice = CreateInvoice();
            var exporter = new UblExporter();

            var exportedDocument = await exporter.ExportAsync(invoice, TestContext.Current.CancellationToken);

            var outputDirectory = Path.Combine(
                AppContext.BaseDirectory,
                "GeneratedUblInvoices");

            Directory.CreateDirectory(outputDirectory);

            var outputPath = Path.Combine(
                outputDirectory,
                exportedDocument.FileName);

            await File.WriteAllBytesAsync(outputPath, exportedDocument.Content, TestContext.Current.CancellationToken);

            _output.WriteLine(
                $"Generated UBL XML invoice: {outputPath}");

            Assert.True(File.Exists(outputPath));
            Assert.True(new FileInfo(outputPath).Length > 0);

            Assert.Equal("INV-001.xml", exportedDocument.FileName);
            Assert.Equal("application/xml", exportedDocument.ContentType);
            Assert.Equal("UBL", exportedDocument.Format);
            Assert.Equal("UTF-8", exportedDocument.Encoding);

            var xml = await File.ReadAllTextAsync(outputPath, Encoding.UTF8, TestContext.Current.CancellationToken);
            var document = XDocument.Parse(xml);

            XNamespace invoiceNs =
                "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";

            XNamespace cbc =
                "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";

            XNamespace cac =
                "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            Assert.Equal(
                invoiceNs + "Invoice",
                document.Root!.Name);

            Assert.Equal(
                "INV-001",
                document.Root.Element(cbc + "ID")!.Value);

            Assert.Equal(
                "380",
                document.Root.Element(cbc + "InvoiceTypeCode")!.Value);

            Assert.Equal(
                "EUR",
                document.Root.Element(cbc + "DocumentCurrencyCode")!.Value);

            Assert.NotNull(
                document.Root.Element(cac + "AccountingSupplierParty"));

            Assert.NotNull(
                document.Root.Element(cac + "AccountingCustomerParty"));

            Assert.NotNull(
                document.Root.Element(cac + "TaxTotal"));

            Assert.NotNull(
                document.Root.Element(cac + "LegalMonetaryTotal"));

            Assert.NotNull(
                document.Root.Element(cac + "InvoiceLine"));
        }

        private static CommercialInvoice CreateInvoice()
        {
            const string currency = "EUR";

            return new CommercialInvoice
            {
                InvoiceId = new InvoiceNumber("INV-001"),

                IssueDate = new IssueDate(
                    new DateOnly(2026, 1, 15)),

                DueDate = new DueDate(
                    new DateOnly(2026, 2, 15)),

                Note = new Note("Generated test invoice"),

                DocumentCurrencyCode =
                    new DocumentCurrencyCode(currency),

                AccountingSupplierParty =
                    CreateSupplierParty(),

                AccountingCustomerParty =
                    CreateCustomerParty(),

                TaxTotals = new List<TaxTotal>
                {
                    new(
                        Amount: new TaxAmount(
                            Amount: 27m,
                            CurrencyId: currency),

                        SubTotal: new List<TaxSubtotal>
                        {
                            new(
                                TaxableAmount: new TaxableAmount(
                                    Amount: 100m,
                                    CurrencyId: currency),

                                Amount: new TaxAmount(
                                    Amount: 27m,
                                    CurrencyId: currency),

                                Category: new TaxCategory(
                                    VatCategoryCode: "S",
                                    Percent: 27f,
                                    TaxExemptionReasonCode: null,
                                    TaxExemptionReason: null,
                                    TaxScheme: "VAT"))
                        })
                },

                LegalMonetaryTotal = new LegalMonetaryTotal(
                    LineExtensionAmount: new LineExtensionAmount(
                        Amount: 100m,
                        CurrencyId: currency),

                    TaxExclusiveAmount: new TaxExclusiveAmount(
                        Amount: 100m,
                        CurrencyId: currency),

                    TaxInclusiveAmount: new TaxInclusiveAmount(
                        Amount: 127m,
                        CurrencyId: currency),

                    AllowanceTotalAmount: null,
                    ChargeTotalAmount: null,
                    PrePaidAmount: null,
                    PayableRoundAmount: null,

                    PayableAmount: new PayableAmount(
                        Amount: 127m,
                        CurrencyId: currency)),

                InvoiceLines = new List<InvoiceLine>
                {
                    new(
                        LineId: "1",

                        LineNote: new Note("Test invoice line"),

                        InvoicedQuantity: new InvoicedQuantity(
                            Quantity: 1,
                            UnitCode: "EA"),

                        LineExtensionAmount:
                            new LineExtensionAmount(
                                Amount: 100m,
                                CurrencyId: currency),

                        InvoicePeriod: null,

                        Kind: new Item(
                            Description: "Consulting service",
                            Name: "Consulting",
                            BuyersItemIdentification: null,
                            SellersItemIdentification: "SKU-001",
                            StandardItemIdentification: null,
                            OriginCountryCode: "HU",

                            ClassifiedTaxCategory:
                                new ClassifiedTaxCategory(
                                    VatCategoryCode: "S",
                                    Percent: 27f,
                                    TaxScheme: "VAT"),

                            AIProperty: new List<AdditionalItemProperty>
                            {
                                new(
                                    Name: "Project",
                                    Value: "Invoice processing pipeline")
                            }),

                        ItemPrice: new Price(
                            PAmount: new PriceAmount(
                                Amount: 100m,
                                CurrencyId: currency),

                            BaseQuantity: new BaseQuantity(
                                Quantity: 1,
                                UnitCode: "EA"),

                            AllowanceCharge: null),

                        AllowanceCharges: null)
                }
            };
        }

        private static DomainParty CreateSupplierParty()
        {
            return new DomainParty
            {
                Name = new PartyName("Supplier Kft."),

                Address = new PostalAddress(
                    StreetName: "Supplier Street 1",
                    AdditionalStreetName: null,
                    CityName: "Budapest",
                    PostalZone: "1111",
                    CountrySubentity: null,
                    AddressLine: null,
                    CountryIdentificationCode: "HU"),

                PartyTaxSchemes = new List<PartyTaxScheme>
                {
                    new(
                        CompanyID: "HU12345678",
                        TaxScheme: "VAT")
                },

                PartyLegalEntity = new PartyLegalEntity(
                    PartyRegistrationName: "Supplier Kft.",
                    CompanyId: "12345678-2-42",
                    CompanyLegalForm: "Kft."),

                ContactInfo = new Contact(
                    Name: "Supplier Contact",
                    Telephone: "+3612345678",
                    ElectronicMail: "supplier@example.com")
            };
        }

        private static DomainParty CreateCustomerParty()
        {
            return new DomainParty
            {
                Name = new PartyName("Customer GmbH"),

                Address = new PostalAddress(
                    StreetName: "Customer Street 1",
                    AdditionalStreetName: null,
                    CityName: "Vienna",
                    PostalZone: "1010",
                    CountrySubentity: null,
                    AddressLine: null,
                    CountryIdentificationCode: "AT"),

                PartyTaxSchemes = new List<PartyTaxScheme>
                {
                    new(
                        CompanyID: "ATU12345678",
                        TaxScheme: "VAT")
                },

                PartyLegalEntity = new PartyLegalEntity(
                    PartyRegistrationName: "Customer GmbH",
                    CompanyId: "ATU12345678",
                    CompanyLegalForm: "GmbH"),

                ContactInfo = new Contact(
                    Name: "Customer Contact",
                    Telephone: "+4312345678",
                    ElectronicMail: "customer@example.com")
            };
        }

        [Fact]
        public async Task Export_ShouldGenerateXsdValidUbl21InvoiceXmlFile()
        {
            // Arrange
            var invoice = CreateInvoice();
            var exporter = new UblExporter();

            // Act
            var exportedDocument = await exporter.ExportAsync(invoice, TestContext.Current.CancellationToken);

            var outputDirectory = Path.Combine(
                AppContext.BaseDirectory,
                "GeneratedUblInvoices");

            Directory.CreateDirectory(outputDirectory);

            var outputPath = Path.Combine(
                outputDirectory,
                exportedDocument.FileName);

            File.WriteAllBytes(
                outputPath,
                exportedDocument.Content);

            _output.WriteLine(
                $"Generated UBL XML invoice: {outputPath}");

            // Ellenőrizzük, hogy a fájl valóban létrejött.
            Assert.True(
                File.Exists(outputPath),
                $"Az XML fájl nem jött létre: {outputPath}");

            Assert.True(
                new FileInfo(outputPath).Length > 0,
                "A létrehozott XML fájl üres.");

            // A ténylegesen exportált XML beolvasása.
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };

            using (var stream = new MemoryStream(
                       exportedDocument.Content,
                       writable: false))
            {
                xmlDocument.Load(stream);
            }

            // UBL 2.1 XSD-validáció.
            var validationResults = UblDocumentValidator.Default
                .Validate(
                    xmlDocument,
                    suppressWarnings: false)
                .ToList();

            // A warningokat és hibákat kiírjuk a teszt outputjába.
            foreach (var result in validationResults)
            {
                _output.WriteLine(
                    $"{result.Severity}: " +
                    $"{result.Message} " +
                    $"(sor: {result.Exception.LineNumber}, " +
                    $"pozíció: {result.Exception.LinePosition})");
            }

            var validationErrors = validationResults
                .Where(result =>
                    result.Severity == XmlSeverityType.Error)
                .ToList();

            var errorMessage = string.Join(
                Environment.NewLine,
                validationErrors.Select(result =>
                    $"{result.Severity}: {result.Message} " +
                    $"(sor: {result.Exception.LineNumber}, " +
                    $"pozíció: {result.Exception.LinePosition})"));

            Assert.True(
                validationErrors.Count == 0,
                "Az előállított XML nem felel meg az UBL 2.1 XSD-nek." +
                Environment.NewLine +
                errorMessage);

            _output.WriteLine(
                "Az XML megfelel az UBL 2.1 XSD-nek.");
        }
    }
}