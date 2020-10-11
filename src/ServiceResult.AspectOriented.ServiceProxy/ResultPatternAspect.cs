using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using ServiceResult.AspectOriented.ServiceProxy.Emit;
using ServiceResult.AspectOriented.ServiceProxy.Extensions;
using ServiceResult.Exceptions;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    public class ResultPatternAspect<TService> : GenericDecorator<TService>, IResultPatternService, IResultPatternAspect<TService>
    {
        private IObjectBuilder _objectBuilder;
        private IHostEnvironment _hostEnvironment;

        // protected override void BeforeInvoke(MethodInfo targetMethod, object[] args, MethodKind methodKind) => base.BeforeInvoke(targetMethod, args, methodKind);
        protected override object AfterInvoke(MethodInfo targetMethod, object[] args, MethodKind methodKind, object result)
        {
            if (methodKind != MethodKind.Method)
            {
                return result;
            }

            var dynamicResult = CreateProxyToObject(targetMethod, result);

            dynamicResult.Kind = ResultKinds.Success;
            dynamicResult.WarningDescription = null;
            dynamicResult.ExceptionDescriptions = null;

            return dynamicResult;
        }

        protected override object OnException(Exception exception, MethodInfo methodInfo, out bool handled)
        {
            var warningException = exception as WarningException;

            handled = true;

            var dynamicResult = CreateProxyToObject(methodInfo, warningException?.Result);

            dynamicResult.Kind = warningException is null ? ResultKinds.Exception : ResultKinds.Warning;
            dynamicResult.WarningDescription = warningException?.Message;
            dynamicResult.ExceptionDescriptions = new ExceptionDescriptions(exception as ServiceException ?? new InternalServerErrorException(exception.Message, exception), methodInfo.Name, _hostEnvironment);

            return dynamicResult;
        }

        private IResultPatternProxy CreateProxyToObject(MethodInfo targetMethod, object result)
        {
            var returnType = GetMethodReturnType(targetMethod);
            var dynamicResult = _objectBuilder.CreateObject(CreateTypeName(returnType), null, returnType, new[] { typeof(IResultPatternProxy) }, true) as IResultPatternProxy;

            if (result != null)
            {
                var mapperConfiguration = new AutoMapper.MapperConfiguration(config => config.CreateMap(returnType, dynamicResult.GetType()));
                var mapper = new AutoMapper.Mapper(mapperConfiguration);

                mapper.Map(result, dynamicResult);
                dynamicResult.IsNull = false;

                var hh = returnType.IsAssignableFrom(dynamicResult.GetType());
            }

            dynamicResult.IsNull = true;

            return dynamicResult;
        }

        private static string CreateTypeName(Type type) => $"_PROXY_RESULT_PATTERN_{type.Name.ToUpper()}_";

        IResultPatternAspect<TService> IResultPatternAspect<TService>.Initialize(TService service, IObjectBuilder objectBuilder, IHostEnvironment environment)
        {
            SetParameters(service);
            _objectBuilder = objectBuilder;
            _hostEnvironment = environment;

            return this;
        }
    }
}
