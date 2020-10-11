using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using ServiceResult.Domain.Models;
using ServiceResult.Exceptions;

namespace ServiceResult.ResultPattern
{
    public class ExceptionDescriptions : Model
    {
        public ExceptionDescriptions(Exception exception, string title = null, bool mapInnerExceptionMessage = false, bool mapStackTrace = false, bool mapTargetSite = false)
        {
            if (exception == null) { return; }

            var httpStatusCodeException = exception as HttpStatusCodeException;

            Exception = exception;
            Title = title;
            Detail = exception.Message;
            Status = httpStatusCodeException != null ? httpStatusCodeException.HttpStatusCode.ToString() : nameof(HttpStatusCode.InternalServerError);
            StatusCode = httpStatusCodeException != null ? (int)httpStatusCodeException.HttpStatusCode : (int)HttpStatusCode.InternalServerError;
            StackTrace = mapStackTrace && exception.StackTrace != null ? exception.StackTrace.Split(new[] { "\n" }, int.MaxValue, StringSplitOptions.RemoveEmptyEntries) : null;

            if (exception.TargetSite != null)
            {
                TargetSite = mapTargetSite ? $"{exception.TargetSite.ReflectedType.Namespace}.{exception.TargetSite.ReflectedType.Name}.{exception.TargetSite.Name}" : null;
            }

            InnerException = mapInnerExceptionMessage ? GetInnerException(exception) : null;
        }

        public ExceptionDescriptions(Exception exception, string title, IHostEnvironment environment) : this(exception, title, environment.IsDevelopment(), environment.IsDevelopment(), environment.IsDevelopment()) { }

        [JsonIgnore]
        public Exception Exception { get; }

        [DefaultValue(0)]
        public int StatusCode { get; }
        public string Status { get; }
        public string Title { get; }

        /// <summary>
        /// Description of the error or exception message
        /// </summary>
        public string Detail { get; }
        public string TargetSite { get; }
        public ICollection<string> StackTrace { get; }
        public ICollection<string> InnerException { get; }

        private static string[] GetInnerException(Exception exception)
        {
            if (exception?.InnerException == null)
            {
                return null;
            }

            var innerException = new List<string>();

            exception = exception.InnerException;

            while (exception != null)
            {
                innerException.Add(exception.Message);
                exception = exception.InnerException;
            }

            return innerException.ToArray();
        }
    }
}
