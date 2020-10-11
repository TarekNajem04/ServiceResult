using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ServiceResult.ResultPattern.WebApi.Controllers
{
    public abstract class AppController<TService> : AppController where TService : IService
    {
        protected AppController(TService service, IWebHostEnvironment environment) : base(environment) => Service = service;

        protected TService Service { get; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public abstract class AppController : ControllerBase
    {
        protected AppController(IWebHostEnvironment environment) => Environment = environment;

        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Creates an <see cref="ObjectResult"/> object that produces an <see cref="StatusCodes.Status403Forbidden"/> response.
        /// </summary>
        /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
        [NonAction]
        public virtual ObjectResult Forbidden([ActionResultObjectValue] object value) => StatusCode(StatusCodes.Status403Forbidden, value);
        /// <summary>
        /// Creates an <see cref="ObjectResult"/> object that produces an <see cref="StatusCodes.Status500InternalServerError"/> response.
        /// </summary>
        /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
        [NonAction]
        public virtual ObjectResult InternalServerError([ActionResultObjectValue] object value) => StatusCode(StatusCodes.Status500InternalServerError, value);

        /// <summary>
        /// Creates an <see cref="ObjectResult"/> object that produces an <see cref="StatusCodes.Status405MethodNotAllowed"/> response.
        /// </summary>
        /// <returns>The created <see cref="ObjectResult"/> for the response.</returns>
        [NonAction]
        public virtual ObjectResult MethodNotAllowed([ActionResultObjectValue] object value) => StatusCode(StatusCodes.Status405MethodNotAllowed, value);
    }
}
