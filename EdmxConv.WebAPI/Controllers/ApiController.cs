using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace EdmxConv.WebAPI.Controllers
{
    public abstract class ApiController : Controller
    {
        protected IActionResult MapToHttpResponse(Result result) =>
            result.IsSuccess
                ? (IActionResult)Ok(result)
                : (IActionResult)BadRequest(result.Error);
    }
}