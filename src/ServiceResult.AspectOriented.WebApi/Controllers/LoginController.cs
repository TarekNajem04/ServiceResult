using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.AspectOriented.ServiceProxy.Extensions;
using ServiceResult.AspectOriented.WebApi.Extensions;
using ServiceResult.AspectOriented.WebApi.Models;

namespace ServiceResult.AspectOriented.WebApi.Controllers
{
    public class LoginController : AppController<ILoginService>
    {
        public LoginController(ILoginService service, IWebHostEnvironment environment) : base(service, environment) { }

        [AllowAnonymous]
        [HttpPost("AuthenticateAsync")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] LoginModel model) =>
            (
                await Service
                        .AuthenticateAsync(model.UserName, model.Password)
                        .ConfigureAwait(false)
            )
            .GetResult()
            .ToActionResult(this);

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model) =>
            Service
                .Authenticate(model.UserName, model.Password)
                .GetResult()
                .ToActionResult(this);
    }
}