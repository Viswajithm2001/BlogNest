using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace BlogNest.Middlewares
{
    /// <summary>
    /// Middleware component for centralized error handling in the BlogNest application.
    /// Catches unhandled exceptions, logs them, and returns a standardized error response.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        /// <param name="logger">The logger instance for error logging.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Processes an HTTP request, handling any unhandled exceptions that occur during the request pipeline.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// When an unhandled exception occurs:
        /// - Logs the error with full exception details
        /// - Sets response type to JSON
        /// - Returns a 500 Internal Server Error status code
        /// - Includes a standardized error response with status code, message, and trace ID
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later.",
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }

    }
}