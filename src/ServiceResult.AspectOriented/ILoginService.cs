using System.Threading.Tasks;
using ServiceResult.Domain.DataTransferObjects;

namespace ServiceResult.AspectOriented
{
    public interface ILoginService : IService
    {
        Task<LoginDto> AuthenticateAsync(string userName, string pw);
        LoginDto Authenticate(string userName, string pw);
    }
}