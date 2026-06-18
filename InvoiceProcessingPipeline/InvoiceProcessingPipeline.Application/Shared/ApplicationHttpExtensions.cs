using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;

namespace InvoiceProcessingPipeline.Application.Shared;

public static class ApplicationHttpExtensions
{
    public sealed record ApplicationError(
        string Code,
        string Message);

    public readonly record struct Empty;

    public sealed class ApplicationResponse<T>
        where T : notnull
    {
        private readonly HttpRequestData _request;
        private readonly bool _writeBody;

        private readonly Dictionary<string, string> _headers =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly List<Action<HttpResponseData>> _configurators =
            new();

        private bool _isBuilt;

        internal ApplicationResponse(
            HttpRequestData request,
            HttpStatusCode statusCode,
            T data,
            bool writeBody = true)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(data);

            _request = request;
            _writeBody = writeBody;

            StatusCode = statusCode;
            Data = data;
        }

        public T Data { get; }

        public HttpStatusCode StatusCode { get; private set; }

        public IReadOnlyDictionary<string, string> Headers =>
            _headers;

        public ApplicationResponse<T> WithStatus(
            HttpStatusCode statusCode)
        {
            EnsureNotBuilt();

            StatusCode = statusCode;

            return this;
        }

        public ApplicationResponse<T> WithHeader(
            string name,
            string value)
        {
            EnsureNotBuilt();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "The header name cannot be empty.",
                    nameof(name));
            }

            ArgumentNullException.ThrowIfNull(value);

            if (name.Equals(
                    "Content-Type",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Content-Type is managed automatically for JSON responses.");
            }

            _headers[name] = value;

            return this;
        }

        public ApplicationResponse<T> WithHeaders(
            params (string Name, string Value)[] headers)
        {
            EnsureNotBuilt();
            ArgumentNullException.ThrowIfNull(headers);

            foreach ((string name, string value) in headers)
            {
                WithHeader(name, value);
            }

            return this;
        }

        public ApplicationResponse<T> NoStore() =>
            WithHeader(
                "Cache-Control",
                "no-store");

        public ApplicationResponse<T> WithRequestId(
            string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
            {
                throw new ArgumentException(
                    "The request ID cannot be empty.",
                    nameof(requestId));
            }

            return WithHeader(
                "X-Request-Id",
                requestId);
        }

        public ApplicationResponse<T> WithLocation(
            Uri location)
        {
            ArgumentNullException.ThrowIfNull(location);

            return WithHeader(
                "Location",
                location.ToString());
        }

        public ApplicationResponse<T> WithETag(
            string etag)
        {
            if (string.IsNullOrWhiteSpace(etag))
            {
                throw new ArgumentException(
                    "The ETag cannot be empty.",
                    nameof(etag));
            }

            string normalizedETag =
                etag.StartsWith('"') ||
                etag.StartsWith(
                    "W/\"",
                    StringComparison.Ordinal)
                    ? etag
                    : $"\"{etag}\"";

            return WithHeader(
                "ETag",
                normalizedETag);
        }

        public ApplicationResponse<T> WithRetryAfter(
            int seconds)
        {
            if (seconds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(seconds),
                    "Retry-After cannot be negative.");
            }

            return WithHeader(
                "Retry-After",
                seconds.ToString(CultureInfo.InvariantCulture));
        }

        public ApplicationResponse<T> Configure(
            Action<HttpResponseData> configure)
        {
            EnsureNotBuilt();
            ArgumentNullException.ThrowIfNull(configure);

            _configurators.Add(configure);

            return this;
        }

        public async Task<HttpResponseData> BuildAsync(
            CancellationToken cancellationToken = default)
        {
            EnsureNotBuilt();
            _isBuilt = true;

            HttpResponseData response =
                _request.CreateResponse(StatusCode);

            ApplyHeaders(response);
            ApplyConfigurators(response);

            if (!_writeBody ||
                IsHeadRequest() ||
                MustNotContainBody(response.StatusCode))
            {
                return response;
            }

            var body = new ResponseBody<T>(
                Success: IsSuccessful(response.StatusCode),
                Data: Data);

            /*
             * Worker 1.x may reset the status code to 200 when
             * WriteAsJsonAsync is called. Preserve and restore it
             * to remain compatible with both 1.x and 2.x.
             */
            HttpStatusCode finalStatusCode =
                response.StatusCode;

            await response.WriteAsJsonAsync(
                body,
                cancellationToken);

            response.StatusCode =
                finalStatusCode;

            return response;
        }

        private void ApplyHeaders(
            HttpResponseData response)
        {
            foreach ((string name, string value) in _headers)
            {
                response.Headers.Remove(name);
                response.Headers.Add(name, value);
            }
        }

        private void ApplyConfigurators(
            HttpResponseData response)
        {
            foreach (Action<HttpResponseData> configure
                     in _configurators)
            {
                configure(response);
            }
        }

        private bool IsHeadRequest() =>
            string.Equals(
                _request.Method,
                "HEAD",
                StringComparison.OrdinalIgnoreCase);

        private void EnsureNotBuilt()
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException(
                    "The response has already been built.");
            }
        }
    }

    // Successful responses

    public static ApplicationResponse<T> Ok<T>(
        this HttpRequestData request,
        T data)
        where T : notnull =>
        Create(
            request,
            HttpStatusCode.OK,
            data);

    public static ApplicationResponse<T> Created<T>(
        this HttpRequestData request,
        Uri location,
        T data)
        where T : notnull =>
        Create(
                request,
                HttpStatusCode.Created,
                data)
            .WithLocation(location);

    public static ApplicationResponse<T> Accepted<T>(
        this HttpRequestData request,
        T data)
        where T : notnull =>
        Create(
            request,
            HttpStatusCode.Accepted,
            data);

    public static ApplicationResponse<Empty> NoContent(
        this HttpRequestData request) =>
        Create(
            request,
            HttpStatusCode.NoContent,
            new Empty(),
            writeBody: false);

    // Client errors

    public static ApplicationResponse<ApplicationError> BadRequest(
        this HttpRequestData request,
        string message = "The request is invalid.",
        string code = "BAD_REQUEST") =>
        Error(
            request,
            HttpStatusCode.BadRequest,
            code,
            message);

    public static ApplicationResponse<ApplicationError> Unauthorized(
        this HttpRequestData request,
        string message = "Authentication is required.",
        string code = "UNAUTHORIZED") =>
        Error(
                request,
                HttpStatusCode.Unauthorized,
                code,
                message)
            .WithHeader(
                "WWW-Authenticate",
                "Bearer");

    public static ApplicationResponse<ApplicationError> Forbidden(
        this HttpRequestData request,
        string message =
            "You do not have permission to perform this operation.",
        string code = "FORBIDDEN") =>
        Error(
            request,
            HttpStatusCode.Forbidden,
            code,
            message);

    public static ApplicationResponse<ApplicationError> NotFound(
        this HttpRequestData request,
        string message =
            "The requested resource was not found.",
        string code = "NOT_FOUND") =>
        Error(
            request,
            HttpStatusCode.NotFound,
            code,
            message);

    public static ApplicationResponse<ApplicationError> Conflict(
        this HttpRequestData request,
        string message =
            "The request conflicts with the current state of the resource.",
        string code = "CONFLICT") =>
        Error(
            request,
            HttpStatusCode.Conflict,
            code,
            message);

    public static ApplicationResponse<ApplicationError>
        UnprocessableEntity(
            this HttpRequestData request,
            string message =
                "The request could not be processed.",
            string code = "UNPROCESSABLE_ENTITY") =>
        Error(
            request,
            HttpStatusCode.UnprocessableEntity,
            code,
            message);

    public static ApplicationResponse<ApplicationError> TooManyRequests(
        this HttpRequestData request,
        int retryAfterSeconds,
        string message =
            "Too many requests were received. Please try again later.",
        string code = "TOO_MANY_REQUESTS") =>
        Error(
                request,
                HttpStatusCode.TooManyRequests,
                code,
                message)
            .WithRetryAfter(retryAfterSeconds);

    // Server errors

    public static ApplicationResponse<ApplicationError>
        InternalServerError(
            this HttpRequestData request,
            string message =
                "An unexpected internal error occurred.",
            string code = "INTERNAL_SERVER_ERROR") =>
        Error(
            request,
            HttpStatusCode.InternalServerError,
            code,
            message);

    public static ApplicationResponse<ApplicationError>
        ServiceUnavailable(
            this HttpRequestData request,
            int? retryAfterSeconds = null,
            string message =
                "The service is temporarily unavailable.",
            string code = "SERVICE_UNAVAILABLE")
    {
        ApplicationResponse<ApplicationError> response =
            Error(
                request,
                HttpStatusCode.ServiceUnavailable,
                code,
                message);

        return retryAfterSeconds.HasValue
            ? response.WithRetryAfter(retryAfterSeconds.Value)
            : response;
    }

    private static ApplicationResponse<T> Create<T>(
        HttpRequestData request,
        HttpStatusCode statusCode,
        T data,
        bool writeBody = true)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(data);

        return new ApplicationResponse<T>(
            request,
            statusCode,
            data,
            writeBody);
    }

    private static ApplicationResponse<ApplicationError> Error(
        HttpRequestData request,
        HttpStatusCode statusCode,
        string code,
        string message)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "The error code cannot be empty.",
                nameof(code));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(
                "The error message cannot be empty.",
                nameof(message));
        }

        return Create(
            request,
            statusCode,
            new ApplicationError(
                Code: code,
                Message: message));
    }

    private static bool IsSuccessful(
        HttpStatusCode statusCode)
    {
        int code = (int)statusCode;

        return code is >= 200 and <= 299;
    }

    private static bool MustNotContainBody(
        HttpStatusCode statusCode)
    {
        int code = (int)statusCode;

        return code is >= 100 and < 200
            or 204
            or 304;
    }

    private sealed record ResponseBody<T>(
        bool Success,
        T Data);
}