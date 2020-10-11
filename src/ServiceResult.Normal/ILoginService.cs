using ServiceResult.Domain.DataTransferObjects;

namespace ServiceResult.Normal
{
    public interface ILoginService : IService
    {
        LoginDto Authenticate(string userName, string pw);
    }
}