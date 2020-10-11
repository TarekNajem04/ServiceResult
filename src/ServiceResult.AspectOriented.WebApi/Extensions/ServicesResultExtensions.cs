using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.Exceptions;
using ServiceResult.AspectOriented.WebApi.Controllers;
using ServiceResult.AspectOriented.ServiceProxy;

namespace ServiceResult.AspectOriented.WebApi.Extensions
{
    public static class ServicesResultExtensions
    {
        public static IActionResult ToActionResult<T>(this ServiceResult<T> result, AppController controller, [CallerMemberName] string callerName = "") =>
            // C# 8.0
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression
            // In the Front-End program, we test the value of the Kind property
            // For the object we received via Response,
            // and on the basis of it we build the result for the user.
            result?.Kind switch
            {
                ResultKinds.Exception =>
                        result.ExceptionDescriptions?.Exception switch
                        {
                            EntityNotFoundException _ => controller.NotFound(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            InternalServerErrorException _ => controller.InternalServerError(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            MethodNotAllowedException _ => controller.MethodNotAllowed(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            UnprocessableEntityException _ => controller.UnprocessableEntity(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            BadRequestException _ => controller.BadRequest(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            ForbiddenException _ => controller.Forbidden(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                            _ => controller.InternalServerError(new { result.Kind, KindName = result.Kind.ToString(), result.ExceptionDescriptions }),
                        },
                ResultKinds.Warning => controller.Ok(new { result.Kind, KindName = result.Kind.ToString(), result.Result, result.WarningDescription }),
                ResultKinds.Success => controller.Ok(new { result.Kind, KindName = result.Kind.ToString(), result.Result }),
                _ => controller.InternalServerError(
                        new
                        {
                            Kind = ResultKinds.Exception,
                            KindName = result.Kind.ToString(),
                            ExceptionDescriptions = new ExceptionDescriptions(new InternalServerErrorException(), callerName, controller.Environment)
                        })
            };
    }
}
