using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mockingjay.Common;
using Mockingjay.Common.Json;

namespace MockingjayApp.Middleware
{
    [SuppressMessage(
        "Minor Code Smell",
        "S2221:\"Exception\" should not be caught when not required by called methods",
        Justification = "This is a catch all error handler.")]
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;

            var options = new JsonSerializerOptions().SetApplicationDefaultSettings();
            var result = string.Empty;

            if (exception is ValidationException validationException)
            {
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors, options);
            }
            else if (exception is NotFoundException _)
            {
                code = HttpStatusCode.NotFound;
            }
            else
            {
                code = HttpStatusCode.InternalServerError;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(new { error = exception.Message }, options);
            }

            await context.Response.WriteAsync(result);
        }
    }
}
