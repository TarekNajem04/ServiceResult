using System;

namespace ServiceResult.AspectOriented.ServiceProxy.Extensions
{
    public static class ServiceResultExtensions
    {
        // Probably one of the exceptions we designed, so we test the exception first.
        public static ServiceResult<T> GetResult<T>(this T result) => result is IResultPatternProxy proxy
                ? CreateServiceResult<T>(proxy.Kind, result, proxy.WarningDescription, proxy.ExceptionDescriptions)
                : ServiceResult<T>.Success(result);

        private static ServiceResult<T> CreateServiceResult<T>(ResultKinds kind, object result, string warning, ExceptionDescriptions exceptionDescriptions) =>
            Activator.CreateInstance(typeof(ServiceResult<>).MakeGenericType(typeof(T)), kind, result, warning, exceptionDescriptions) as ServiceResult<T>;
    }
}
