using Nowadays.Common.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Net;
using System.Net.Mime;

namespace Nowadays.API.Exceptions
{
    public static class ApplicationBuilderException
    {
        private static readonly Serilog.ILogger _logger = Log.ForContext(typeof(ApplicationBuilderException));

        // includeExceptionDetails:false => To avoid returning all the details when an exception occurs
        public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app, bool includeExceptionDetails = false, bool useDefaultHandlingResponse = true, Func<HttpContext, Exception, Task> handleException = null!)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(context =>
                {
                    var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

                    // If I want it to behave differently when I add a new exception type to my system in the future, I can come and add it here.
                    // useDefaultHandlingResponse is false, that is, if he does not want to use the function that I will write himself, but he has not sent the one he wants to use.(handleException == null):

                    if (!useDefaultHandlingResponse && handleException == null)
                        throw new ArgumentNullException(nameof(handleException), $"{nameof(handleException)} cannot be null when {nameof(useDefaultHandlingResponse)} is false. ");

                    if (!useDefaultHandlingResponse && handleException != null)
                        return handleException(context, exceptionObject!.Error);

                    // If it doesn't meet either condition, I'll run my DefaulaHandle.
                    return DefaultHandleException(context, exceptionObject!.Error, includeExceptionDetails);
                });

            });

            return app;
        }

        private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExceptionDetails = false)
        {
            // Ben aksini söyleyene kadar message ve statusCode olarak bunlar dönsün.
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal server error occured!";
            string errorDetail = exception.InnerException is not null ? exception.InnerException.Message : null!;


            if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access!";
                _logger.Error($"UnauthorizedAccessException:", message);
                return;
            }

            if (exception is UserCreateFailedException)
            {
                var validationResponse = new ValidationResponseModel(exception.Message, httpStatusCode: (int)statusCode, errorDetails: errorDetail!);

                _logger.Error($"UserCreate Failed Exception: {exception.Message}");

                await WriteResponse(context, statusCode, validationResponse);
                return;
            }

            // In the case of 'DatabaseValidationException', I used this if statement to throw a different exception.
            if (exception is DatabaseValidationException)
            {
                var validationResponse = new ValidationResponseModel(exception.Message, httpStatusCode: (int)statusCode, errorDetails: errorDetail!);

                _logger.Error($"Database validation error: {exception.Message}");

                await WriteResponse(context, statusCode, validationResponse);
                return;
            }

            //ValidationResponseModel res = new ValidationResponseModel
            //{
            //    Errors = new[] { message},
            //    HttpStatusCode = (int)statusCode,
            //    ErrorDetails = errorDetail
            //};

            // I create a dynamic validation for the remaining cases where the conditions in the if statements above are not met. (To see all the details myself.)
            var res = new
            {
                HttpStatusCode = (int)statusCode,
                Detail = includeExceptionDetails ? exception.ToString() : message
            };
            await WriteResponse(context, statusCode, res);
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObj)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(responseObj);
        }
    }
}
