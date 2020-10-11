namespace ServiceResult.AspectOriented.ServiceProxy
{
    public interface IResultPatternAspecFactory
    {
        TService Create<TService>(TService service);
    }
}
