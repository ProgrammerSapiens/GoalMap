using System.Net;
using System.Text.Json;

namespace API
{
    /// <summary>
    /// Middleware for handling global exceptions in the application.
    /// Catches unhandled exceptions, logs them, and returns a standardized JSON response.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware entry point that processes HTTP requests and catches unhandled exceptions.
        /// </summary>
        /// <param name="context">The HTTP context of the request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exceptions by setting an appropriate HTTP status code and returning a JSON-formatted error response.
        /// </summary>
        /// <param name="context">The HTTP context of the request.</param>
        /// <param name="exception">The caught exception.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            response.StatusCode = statusCode;

            var errorResponse = new
            {
                message = exception.Message,
                statusCode = statusCode,
                details = exception.StackTrace
            };

            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
