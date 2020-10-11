using ServiceResult.Domain.DataTransferObjects;

namespace ServiceResult.ExceptionPattern
{
    public interface ILoginService : IService
    {
        LoginDto Authenticate(string userName, string pw);
    }
}