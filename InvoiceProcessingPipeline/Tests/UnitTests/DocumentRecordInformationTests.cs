using InvoiceProcessingPipeline.Application.DocumentAudit;
using InvoiceProcessingPipeline.Application.DTOs;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Xunit;

namespace Tests.UnitTests
{
    public sealed class DocumentRecordInformationTests
    {
        private static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                Converters =
                {
                new JsonStringEnumConverter(
                    JsonNamingPolicy.CamelCase,
                    allowIntegerValues: false)
                }
            };

        public static TheoryData<
            AuditStatus,
            string,
            string,
            string?,
            string> SerializationCases =>
            new()
            {
            {
                AuditStatus.APPROVED,
                "workflow-001",
                "7ec85d8d-8e22-42ee-87bc-b268ba38f67b",
                null,
                """
                {
                  "invoiceStatus": "approved",
                  "workflowId": "workflow-001",
                  "auditId": "7ec85d8d-8e22-42ee-87bc-b268ba38f67b"
                }
                """
            },
            {
                AuditStatus.REJECTED,
                "workflow-002",
                "67bd7644-9aad-4171-bcbd-02fb17424ef4",
                "INV-2026-001",
                """
                {
                  "invoiceStatus": "rejected",
                  "workflowId": "workflow-002",
                  "auditId": "67bd7644-9aad-4171-bcbd-02fb17424ef4",
                  "invoiceNumber": "INV-2026-001"
                }
                """
            },
            {
                AuditStatus.UNDER_REVIEW,
                "workflow-003",
                "ec93331f-e4ec-41b3-906d-1a65f5acfd03",
                "",
                """
                {
                  "invoiceStatus": "pending",
                  "workflowId": "workflow-003",
                  "auditId": "ec93331f-e4ec-41b3-906d-1a65f5acfd03",
                  "invoiceNumber": ""
                }
                """
            }
            };

        [Theory]
        [MemberData(nameof(SerializationCases))]
        public void Serialize_WithDifferentValues_ProducesExpectedJson(
            AuditStatus auditStatus,
            string processId,
            string documentId,
            string? invoiceId,
            string expectedJson)
        {
            // Arrange
            var record = new DocumentRecordInformation
            {
                AuditStatus = auditStatus,
                ProcessId = processId,
                DocumentId = documentId,
                InvoiceId = invoiceId
            };

            // Act
            var actualJson = JsonSerializer.Serialize(
                record,
                SerializerOptions);

            // Assert
            var expectedNode = JsonNode.Parse(expectedJson);
            var actualNode = JsonNode.Parse(actualJson);

            Assert.True(
                JsonNode.DeepEquals(expectedNode, actualNode),
                $"""
            A szerializált JSON nem megfelelő.

            Elvárt:
            {expectedJson}

            Tényleges:
            {actualJson}
            """);
        }
    }
}