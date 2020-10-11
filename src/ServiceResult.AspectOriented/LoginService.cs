using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;
using ServiceResult.Exceptions;

namespace ServiceResult.AspectOriented
{
    public class LoginService : Service, ILoginService
    {
        public LoginService(IMapper mapper) : base(mapper) { }

        [DebuggerHidden]
        public Task<LoginDto> AuthenticateAsync(string userName, string pw) => Task.Run(() => Authenticate(userName, pw));

        [DebuggerHidden]
        public LoginDto Authenticate(string userName, string pw)
        {
            try
            {
                // This is just an example of the return type so we haven't
                // listed all the code this function needs to fetch data
                // from the database and the validation code.

                // ===== User with Warning =====
                if (userName == "user_a" && pw == "pw")
                {
                    // To follow the 'warning convention' between AOP and the service.
                    throw new WarningException(
                           Map<Login, LoginDto>(new Login() { UserName = "Warning user" }),
                           "For more than ten months you have not changed your password, we recommend that you change it as soon as possible.");
                }

                // ===== Valid user =====
                if (userName == "user_b" && pw == "pw")
                {
                    return Map<Login, LoginDto>(new Login() { UserName = "Valid user" });
                }

                // ===== User with Exception =====
                if (userName == "user_c" && pw == "pw")
                {
                    throw new ForbiddenException($"User {userName} is currently blocked.");
                }
            }
            catch (Exception ex)
            {
                // Probably one of the exceptions we designed, so we test the exception first.
                throw ex as HttpStatusCodeException ?? new InternalServerErrorException(ex.Message, ex);

                // 
                /*
                 * ====================================================
                 * During the troubleshooting process, we can throw other types, when analyzing this exception 'ex'.
                 * ====================================================
                 * if (ex is xxxException) {
                 *      throw xxx_Custme_Exception(ex.Message, ex), ...));
                 * }
                 * ...
                 */
            }

            throw new BadRequestException("The username or password you entered is incorrect.");
        }
    }
}