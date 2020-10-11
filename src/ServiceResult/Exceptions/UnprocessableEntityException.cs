using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class UnprocessableEntityException : HttpStatusCodeException
    {
        public UnprocessableEntityException() { }
        public UnprocessableEntityException(string message) : base(message, HttpStatusCode.UnprocessableEntity) { }
        public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
