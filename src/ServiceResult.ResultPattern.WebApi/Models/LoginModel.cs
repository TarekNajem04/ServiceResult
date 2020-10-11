using ServiceResult.Domain.Models;

namespace ServiceResult.ResultPattern.WebApi.Models
{
    public class LoginModel : Model
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}