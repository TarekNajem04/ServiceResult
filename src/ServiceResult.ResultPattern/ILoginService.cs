using ServiceResult.Domain.DataTransferObjects;

namespace ServiceResult.ResultPattern
{
    public interface ILoginService : IService
    {
        ServiceResult<LoginDto> Authenticate(string userName, string pw);
    }
}