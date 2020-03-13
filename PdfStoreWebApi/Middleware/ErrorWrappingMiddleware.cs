using System;
using System.Net;
using System.Threading.Tasks;
using DocumentStore.Application.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocumentStore.WebApi.Middleware
{
    /// <summary>
    /// Custom error handler Middleware.
    /// </summary>
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorWrappingMiddleware> logger;
        private const string JsonContentType = "application/json";

        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception exception)
            {
                logger.Log(LogLevel.Error, $"Exception happened when handling request {context.Request.Path + context.Request.QueryString}\nException: {exception}");

                if (!context.Response.HasStarted)
                    context.Response.Clear();

                var httpStatusCode = MapExceptionTypeToStatusCode(exception);

                // set http status code and content type
                context.Response.StatusCode = httpStatusCode;
                context.Response.ContentType = JsonContentType;
                // writes / returns error model to the response
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new ErrorModelViewModel
                    {
                        Message = exception.Message
                    }));
            }
        }

        /// <summary>
        /// Map specific exception to Status code.
        /// </summary>
        /// <param name="exception">Errors that occur during application execution.</param>
        /// <returns></returns>
        private static int MapExceptionTypeToStatusCode(Exception exception)
        {
            int httpStatusCode;

            // Exception type To Http Status configuration 
            switch (exception)
            {
                case var _ when exception is ValidationException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return httpStatusCode;
        }
    }

    /// <summary>
    /// Exception model.
    /// </summary>
    public class ErrorModelViewModel
    {
        /// <summary>
        /// Exception message.
        /// </summary>
        public string Message { get; set; }
    }
}
