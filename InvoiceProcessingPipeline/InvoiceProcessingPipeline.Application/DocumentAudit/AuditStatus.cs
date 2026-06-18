using System.Text.Json.Serialization;

namespace InvoiceProcessingPipeline.Application.DocumentAudit
{
    /// <summary>
    /// Represents the audit status of a document in the invoice processing pipeline. This status is used to track the current state of the document as it goes through various stages of review and approval. 
    /// The statuses include:
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>
    ///             <description>
    ///                 EXTRACTED - the document is extracted from the raw document and has been canonicalized
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 FAILED - the document processing workflow failed for an unexpected condition
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 CONSTRAINT_VIOLATION - the format of the document is not accepted or some value are missing
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 UNDER_REVIEW - someone is working on the document
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 REJECTED - the document is not eligible for the audit workflow, because of unacceptable preconditions
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 APPROVED - the document is ready for exporting
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 BOOKED - the final verified document is exported and stored
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    [JsonConverter(typeof(JsonStringEnumConverter<AuditStatus>))]
    public enum AuditStatus : byte
    {
        [JsonStringEnumMemberName("extracted")]
        EXTRACTED,
        [JsonStringEnumMemberName("failed")]
        FAILED,
        [JsonStringEnumMemberName("constraint_violation")]
        CONSTRAINT_VIOLATION,
        [JsonStringEnumMemberName("under_review")]
        UNDER_REVIEW,
        [JsonStringEnumMemberName("rejected")]
        REJECTED,
        [JsonStringEnumMemberName("approved")]
        APPROVED,
        [JsonStringEnumMemberName("booked")]
        BOOKED
    }
}
