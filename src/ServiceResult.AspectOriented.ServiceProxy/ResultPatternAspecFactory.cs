using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceResult.AspectOriented.ServiceProxy.Emit;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    public class ResultPatternAspecFactory : IResultPatternAspecFactory
    {
        public ResultPatternAspecFactory(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        public IServiceProvider ServiceProvider { get; }

        public TService Create<TService>(TService service)
        {
            var objectBuilder = ServiceProvider.GetService<IObjectBuilder>();
            var environment = ServiceProvider.GetService<IHostEnvironment>();
            var proxy = DispatchProxy.Create<TService, ResultPatternAspect<TService>>() as IResultPatternAspect<TService>;

            proxy.Initialize(service, objectBuilder, environment);

            return proxy is TService serviceProxy ? serviceProxy : service;
        }
    }
}
