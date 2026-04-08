namespace InvoiceProcessingPipeline.Application.Shared;

public sealed record ActivityResult<T>
{
    public required bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? ErrorMessage { get; init; }

    public static ActivityResult<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static ActivityResult<T> Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}