using System.Net;

namespace ServiceResult.Exceptions
{
    // This is a special class that we will use when we want to add a warning to the ServiceResult,
    // When we implement the Result Pattern
    public class WarningException : HttpStatusCodeException
    {
        public WarningException(object resul, string message) : base(message, HttpStatusCode.OK) => Result = resul;

        public object Result { get; }
    }
}
