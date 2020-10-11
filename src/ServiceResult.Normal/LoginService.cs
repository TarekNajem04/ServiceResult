using AutoMapper;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;

namespace ServiceResult.Normal
{
    public class LoginService : Service, ILoginService
    {
        public LoginService(IMapper mapper) : base(mapper) { }

        public LoginDto Authenticate(string userName, string pw)
        {
            try
            {
                // This is just an example of the return type so we haven't
                // listed all the code this function needs to fetch data
                // from the database and the validation code.

                return userName == "user_b" && pw == "pw" ? Map<Login, LoginDto>(new Login() { UserName = "default user" }) : null;
            }
            catch
            {
                // Is this result sufficient?
                return null;
            }
        }
    }
}