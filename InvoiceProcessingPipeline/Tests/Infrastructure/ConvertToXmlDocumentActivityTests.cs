using InvoiceProcessingPipeline.Application.ExportTypes;
using InvoiceProcessingPipeline.Application.Ports;
using InvoiceProcessingPipeline.Domain.Aggregates.DocumentTypes;
using InvoiceProcessingPipeline.Functions.Activities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Text;

namespace Tests.Infrastructure
{
    public sealed class ExportXmlDocumentActivityTests
    {
        private readonly Mock<IDocumentDataStore> _documentStoreMock;
        private readonly Mock<IDocumentSchemeExporter> _exporterMock;
        private readonly Mock<IDocumentXmlStore> _xmlStoreMock;

        private readonly ExportXmlDocumentActivity _sut;

        public ExportXmlDocumentActivityTests()
        {
            _documentStoreMock =
                new Mock<IDocumentDataStore>(MockBehavior.Strict);

            _exporterMock =
                new Mock<IDocumentSchemeExporter>(MockBehavior.Strict);

            _xmlStoreMock =
                new Mock<IDocumentXmlStore>(MockBehavior.Strict);

            _sut = new ExportXmlDocumentActivity(
                NullLogger<ExportXmlDocumentActivity>.Instance,
                _documentStoreMock.Object,
                _exporterMock.Object,
                _xmlStoreMock.Object);
        }

        [Fact]
        public async Task RunAsync_WhenDocumentExists_ShouldExportAndStoreXml()
        {
            // Arrange
            const string documentId = "document-001";

            var document = new CommercialInvoice();

            var exportedDocument = new ExportedDocument
            {
                FileName = "INV-001.xml",
                ContentType = "application/xml",
                Format = "UBL",
                Encoding = "UTF-8",
                Content = Encoding.UTF8.GetBytes(
                    "<Invoice><ID>INV-001</ID></Invoice>")
            };

            var storedBlobUri = new Uri(
                "https://storage.example/documents-xml/INV-001.xml");

            _documentStoreMock
                .Setup(store =>
                    store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(
                        documentId))
                .ReturnsAsync(document);

            _exporterMock
                .Setup(exporter =>
                    exporter.ExportAsync(
                        document,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(exportedDocument);

            _xmlStoreMock
                .Setup(store =>
                    store.StoreXmlDocumentSchemeAsync(
                        exportedDocument,
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(storedBlobUri);

            // Act
            await _sut.RunAsync(documentId);

            // Assert
            _documentStoreMock.Verify(
                store =>
                    store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(
                        documentId),
                Times.Once);

            _exporterMock.Verify(
                exporter =>
                    exporter.ExportAsync(
                        document,
                        It.IsAny<CancellationToken>()),
                Times.Once);

            _xmlStoreMock.Verify(
                store =>
                    store.StoreXmlDocumentSchemeAsync(
                        exportedDocument,
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RunAsync_WhenDocumentDoesNotExist_ShouldNotExportOrStoreXml()
        {
            // Arrange
            const string documentId = "missing-document";

            _documentStoreMock
                .Setup(store =>
                    store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(
                        documentId))
                .ReturnsAsync((CommercialInvoice)null!);

            // Act
            await _sut.RunAsync(documentId);

            // Assert
            _documentStoreMock.Verify(
                store =>
                    store.RetrieveCanonicalizedDocumentSchemeAsync<CommercialInvoice>(
                        documentId),
                Times.Once);

            _exporterMock.Verify(
                exporter =>
                    exporter.ExportAsync(
                        It.IsAny<CommercialInvoice>(),
                        It.IsAny<CancellationToken>()),
                Times.Never);

            _xmlStoreMock.Verify(
                store =>
                    store.StoreXmlDocumentSchemeAsync(
                        It.IsAny<ExportedDocument>(),
                        It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}