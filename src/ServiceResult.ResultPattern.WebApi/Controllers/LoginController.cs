using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ResultPattern.WebApi.Extensions;
using ServiceResult.ResultPattern.WebApi.Models;

namespace ServiceResult.ResultPattern.WebApi.Controllers
{
    public class LoginController : AppController<ILoginService>
    {
        public LoginController(ILoginService service, IWebHostEnvironment environment) : base(service, environment) { }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model) =>
            Service
                .Authenticate(model.UserName, model.Password)
                .ToActionResult(this);
    }
}