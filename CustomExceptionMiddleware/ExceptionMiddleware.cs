using Microsoft.AspNetCore.Http;
using Entities.Dtos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ExceptionModel ex)
            {
                _logger.LogError(ex.error.ErrorMessage);
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, ExceptionModel exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.error.ErrorCode;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDto()
            {
                ErrorCode = exception.error.ErrorCode,
                ErrorDescription = exception.error.ErrorDescription,
                ErrorMessage = exception.error.ErrorMessage
            }));
        }
    }
}