using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    /// <summary>
    /// Base class for Asp.net WEB API applications
    /// </summary>
    public class HttpStatusCodeException : ServiceException
    {
        public HttpStatusCodeException() { }
        public HttpStatusCodeException(string message) : base(message) { }
        public HttpStatusCodeException(string message, HttpStatusCode httpStatusCode) : this(message, null, httpStatusCode) { }
        public HttpStatusCodeException(string message, Exception innerException) : this(message, innerException, HttpStatusCode.InternalServerError) { }
        public HttpStatusCodeException(string message, Exception innerException, HttpStatusCode httpStatusCode) : base(message, innerException) => HttpStatusCode = httpStatusCode;

        public HttpStatusCode HttpStatusCode { get; }
    }
}
