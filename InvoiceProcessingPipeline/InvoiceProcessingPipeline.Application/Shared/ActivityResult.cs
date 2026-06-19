namespace InvoiceProcessingPipeline.Application.Shared
{
    public sealed record ActivityResult<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T? Value { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        private ActivityResult(bool isSuccess, T? value, string? errorMessage, Exception? exception)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static ActivityResult<T> Success(T value)
            => new(true, value, null, null);

        public static ActivityResult<T> Failure(string errorMessage)
            => new(false, default, errorMessage, null);

        public static ActivityResult<T> Failure(Exception exception)
            => new(false, default, exception.Message, exception);

        public static implicit operator ActivityResult<T>(T value)
            => Success(value);
    }
}
