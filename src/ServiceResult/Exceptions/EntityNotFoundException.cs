using System;
using System.Net;

namespace ServiceResult.Exceptions
{
    public class EntityNotFoundException : HttpStatusCodeException
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        public EntityNotFoundException(Type entityType) : base($"The {entityType?.Name ?? "entity"} does not exist.", HttpStatusCode.NotFound) { }
        public EntityNotFoundException(Type entityType, Exception innerException)
            : base($"The {entityType?.Name ?? "entity"} does not exist.", innerException, HttpStatusCode.NotFound) { }
    }
}
