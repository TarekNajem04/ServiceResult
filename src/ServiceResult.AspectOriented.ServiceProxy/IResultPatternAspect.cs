using Microsoft.Extensions.Hosting;
using ServiceResult.AspectOriented.ServiceProxy.Emit;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    public interface IResultPatternAspect<TService> : IResultPatternService
    {
        IResultPatternAspect<TService> Initialize(TService service, IObjectBuilder objectBuilder, IHostEnvironment environment);
    }
}
