using Microsoft.Extensions.DependencyInjection;
using ServiceResult.AspectOriented.ServiceProxy.Emit;

namespace ServiceResult.AspectOriented.ServiceProxy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddResultPattern(this IServiceCollection services)
        {
            services.AddSingleton<IObjectBuilder, SimpleDynamicObjectBuilder>();
            //services.AddSingleton(typeof(IResultPatternAspect<>), typeof(ResultPatternAspect<>));
            services.AddSingleton<IResultPatternAspecFactory, ResultPatternAspecFactory>();

            return services;
        }
    }
}
