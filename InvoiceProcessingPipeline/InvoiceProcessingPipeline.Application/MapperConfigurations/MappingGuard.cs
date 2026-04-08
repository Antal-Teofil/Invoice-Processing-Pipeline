namespace InvoiceProcessingPipeline.Application.MapperConfigurations;

internal static class MappingGuard
{
    public static string Required(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{fieldName} is missing.");
        }

        return value;
    }
}