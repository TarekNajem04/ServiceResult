using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using ServiceResult.Domain.DataTransferObjects;
using ServiceResult.Domain.Entities;
using ServiceResult.Exceptions;

namespace ServiceResult.ResultPattern
{
    public class LoginService : Service, ILoginService
    {
        public LoginService(IMapper mapper, IHostEnvironment environment) : base(mapper) => Environment = environment;

        protected IHostEnvironment Environment { get; }

        public ServiceResult<LoginDto> Authenticate(string userName, string pw)
        {
            try
            {
                // This is just an example of the return type so we haven't
                // listed all the code this function needs to fetch data
                // from the database and the validation code.

                // ===== User with Warning =====
                if (userName == "user_a" && pw == "pw")
                {
                    return ServiceResult<LoginDto>.Warning(
                        Map<Login, LoginDto>(new Login() { UserName = "default user" }),
                        "For more than ten months you have not changed your password, we recommend that you change it as soon as possible.");
                }

                // ===== Valid user =====
                if (userName == "user_b" && pw == "pw")
                {
                    return ServiceResult<LoginDto>.Success(Map<Login, LoginDto>(new Login() { UserName = "default user" }));
                }

                // ===== User with Exception =====
                if (userName == "user_c" && pw == "pw")
                {
                    return ServiceResult<LoginDto>.Exception(new ExceptionDescriptions(
                                new ForbiddenException($"User {userName} is currently blocked."),
                                MethodBase.GetCurrentMethod().Name,
                                Environment));
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<LoginDto>.Exception(new ExceptionDescriptions(
                        // Probably one of the exceptions we designed, so we test the exception first.
                        ex as HttpStatusCodeException ?? new InternalServerErrorException(ex.Message, ex),
                        MethodBase.GetCurrentMethod().Name,
                        Environment));

                // 
                /*
                 * ====================================================
                 * During the troubleshooting process, we can throw other types, when analyzing this exception 'ex'.
                 * ====================================================
                 * if (ex is xxxException) {
                 *      return ServiceResult<LoginDto>.Exception(nnew ExceptionDescriptions(ew xxx_Custme_Exception(ex.Message, ex), ...));
                 * }
                 * ...
                 */
            }

            return ServiceResult<LoginDto>.Exception(new ExceptionDescriptions(
                        new BadRequestException("The username or password you entered is incorrect."),
                        MethodBase.GetCurrentMethod().Name,
                        Environment));
        }
    }
}