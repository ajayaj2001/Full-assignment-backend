using System;
using Entities.Dtos;

namespace CustomExceptionMiddleware
{
    public class ExceptionModel : Exception
    {
        public ErrorDto error { get; set; } = new ErrorDto();

        public ExceptionModel(string message, string description, int statusCode)
        {
            this.error.ErrorCode = statusCode;
            this.error.ErrorMessage = message;
            this.error.ErrorDescription = description;
        }
    }
}