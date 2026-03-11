using System.ComponentModel.DataAnnotations;

namespace InvoiceProcessingPipeline.Infrastructure.Configurations;

public sealed record DocumentStorageOptions
{
    public const string SectionName = "DOCUMENT_STORAGE";

    [Url]
    public required string ServiceUri { get; init; }

    [Range(1, 1440)]
    public int SasTtlMinutes { get; init; } = 10;
}