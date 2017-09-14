using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace EdmxConv.WebAPI.Controllers
{
    public abstract class ApiController : Controller
    {
        protected IActionResult MapToHttpResponse<T>(Result<T> result) =>
            result.IsSuccess
                ? (IActionResult)Ok(result.Value)
                : (IActionResult)BadRequest(result.Error);

        protected IActionResult MapToHttpResponse(Result result) =>
            result.IsSuccess
                ? (IActionResult)Ok()
                : (IActionResult)BadRequest(result.Error);
    }
}