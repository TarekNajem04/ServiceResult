using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServiceResult.Exceptions;

namespace ServiceResult.ExceptionPattern.WebApi.Controllers
{
    public abstract class AppController<TService> : AppController where TService : IService
    {
        protected AppController(TService service) => Service = service;

        protected TService Service { get; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public abstract class AppController : ControllerBase
    {
        protected IActionResult GetResult(Exception exception) =>
            // C# 8.0
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression
            exception switch
            {
                EntityNotFoundException _ => NotFound(exception.Message),
                InternalServerErrorException _ => InternalServerError(exception.Message),
                MethodNotAllowedException _ => MethodNotAllowed(exception.Message),
                UnprocessableEntityException _ => UnprocessableEntity(exception.Message),
                BadRequestException _ => BadRequest(exception.Message),
                ForbiddenException _ => Forbidden(exception.Message),
                WarningException warningException => Warning(warningException),
                _ => InternalServerError(exception.Message),
            };

        [NonAction]
        public virtual ObjectResult Warning(WarningException warningException)
        {
            var expando = new ExpandoObject();

            if (warningException.Result != null)
            {
                var dictionary = (IDictionary<string, object>)expando;

                foreach (var property in warningException.Result.GetType().GetProperties())
                {
                    dictionary.Add(property.Name, property.GetValue(warningException.Result));
                }
            }

            expando.TryAdd("WarningDescription", warningException.Message);

            return Ok(expando);
        }

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
