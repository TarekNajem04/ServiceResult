using System;
using AutoMapper;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;
using ServiceResult.Exceptions;

namespace ServiceResult.ExceptionPattern
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

                // ===== User with Warning =====
                if (userName == "user_a" && pw == "pw")
                {
                    // The ASP.NET Core will treat this warning as an Internal Server Error.
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

                throw new BadRequestException("The username or password you entered is incorrect.");
            }
            catch (Exception ex)
            {
                // Probably one of the exceptions we designed, so we test the exception first.
                throw ex as HttpStatusCodeException ?? new InternalServerErrorException(ex.Message, ex);

                /*
                 * ====================================================
                 * During the troubleshooting process, we can throw other types when  analyzing this Exception.
                 * ====================================================
                 * if (ex is xxxException) {
                 *      throw new A_Custme_Exception(ex.Message, ex);
                 * }
                 * ...
                 */
            }
        }
    }
}