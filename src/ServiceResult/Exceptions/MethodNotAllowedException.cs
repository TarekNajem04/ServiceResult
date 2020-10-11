using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class MethodNotAllowedException : HttpStatusCodeException
    {
        public MethodNotAllowedException() { }
        public MethodNotAllowedException(string message) : base(message, HttpStatusCode.MethodNotAllowed) { }
        public MethodNotAllowedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
