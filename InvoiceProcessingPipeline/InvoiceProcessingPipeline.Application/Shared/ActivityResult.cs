using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceProcessingPipeline.Application.Shared;

public sealed class ActivityResult<T> where T : class
{
    public T? Value { get; init; }
    public ActivityError? Error { get; init; }
    public bool IsSuccess => Error is null;

    public static ActivityResult<T> Success(T value) => new() { Value = value };
    public static ActivityResult<T> Fail(string code, string message) => new() { Error = new ActivityError(code, message) };
}