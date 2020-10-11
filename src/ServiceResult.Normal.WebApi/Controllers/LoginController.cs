using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.Normal.WebApi.Models;

namespace ServiceResult.Normal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service) => _service = service;

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel model)
        {
            var dto = _service.Authenticate(model.UserName, model.Password);

            return dto is null
                ? (IActionResult)BadRequest("The username or password you entered is incorrect.")
                : Ok(dto);
        }
    }
}