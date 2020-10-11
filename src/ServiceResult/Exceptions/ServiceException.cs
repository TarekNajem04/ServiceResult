using System;

namespace ServiceResult.Exceptions
{
    /// <summary>
    /// Base class for all applications
    /// </summary>
    public class ServiceException : Exception
    {
        public ServiceException() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
