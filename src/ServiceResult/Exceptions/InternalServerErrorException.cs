using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class InternalServerErrorException : HttpStatusCodeException
    {
        public InternalServerErrorException() { }
        public InternalServerErrorException(string message) : base(message, HttpStatusCode.InternalServerError) { }
        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
