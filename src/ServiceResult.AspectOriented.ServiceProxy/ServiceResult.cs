using System;
using ServiceResult.Domain.DataTransferObjects;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    /// <summary>
    /// To identify the type of result in the Front-End app.
    /// </summary>
    public enum ResultKinds
    {
        Success,
        Warning,
        Exception
    }

    public class ServiceResult<T> : DTO
    {
        public ServiceResult(ResultKinds kind, T result, string warning, ExceptionDescriptions exceptionDescriptions)
        {
            Result = result;
            ExceptionDescriptions = exceptionDescriptions;
            Kind = kind;
            WarningDescription = warning;
        }

        public ServiceResult(T result) : this(ResultKinds.Success, result, null, null) { }
        public ServiceResult(T result, string warning) : this(ResultKinds.Warning, result, warning, null) { }
        public ServiceResult(ExceptionDescriptions exceptionDescriptions) : this(ResultKinds.Exception, default, null, exceptionDescriptions) { }

        /// <summary>
        /// In the Front-End program, we test the value of the Kind property
        /// For the object we received via Response,
        /// and on the basis of it we build the result for the user.
        /// </summary>
        public ResultKinds Kind { get; }

        /// <summary>
        /// The service result
        /// </summary>
        public T Result { get; }

        /// <summary>
        /// To show warnings in the in the Front-End app.
        /// </summary>
        public string WarningDescription { get; }

        /// <summary>
        /// The exception descriptions
        /// </summary>
        public ExceptionDescriptions ExceptionDescriptions { get; }

        public bool IsSuccess => Kind != ResultKinds.Exception;

        public static ServiceResult<T> Success(T result) => new ServiceResult<T>(ResultKinds.Success, result, null, null);
        public static ServiceResult<T> Warning(T result, string warning) => new ServiceResult<T>(ResultKinds.Warning, result, warning, null);
        public static ServiceResult<T> Exception(ExceptionDescriptions exceptionDescriptions) => new ServiceResult<T>(ResultKinds.Exception, default, null, exceptionDescriptions);

        public static implicit operator T(ServiceResult<T> result) => result.Result;
        public static explicit operator ExceptionDescriptions(ServiceResult<T> result) => result.ExceptionDescriptions;
        public static explicit operator Exception(ServiceResult<T> result) => result.ExceptionDescriptions.Exception;
    }
}
