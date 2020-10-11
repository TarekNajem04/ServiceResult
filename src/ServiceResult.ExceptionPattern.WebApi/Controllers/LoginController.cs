using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ExceptionPattern.WebApi.Models;

namespace ServiceResult.ExceptionPattern.WebApi.Controllers
{
    public class LoginController : AppController<ILoginService>
    {
        public LoginController(ILoginService service) : base(service) { }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model)
        {
            try
            {
                var dto = Service.Authenticate(model.UserName, model.Password);

                /*
                 * In this case, we should not test if the result is null,
                 * because there is a prior agreement between the service and
                 * the caller to catch bugs, if he wants more details about the exceptions.
                 */

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return GetResult(ex);
            }
        }
    }
}