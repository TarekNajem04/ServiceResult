using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class BadRequestException : HttpStatusCodeException
    {
        public BadRequestException() { }
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
