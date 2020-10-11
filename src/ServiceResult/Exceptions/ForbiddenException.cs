using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class ForbiddenException : HttpStatusCodeException
    {
        public ForbiddenException() { }
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden) { }
        public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
