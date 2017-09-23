using CSharpFunctionalExtensions;
using System.Web.Http;

namespace EdmxConv.WebAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult MapToHttpResponse<T>(Result<T> result) =>
            result.IsSuccess
                ? (IHttpActionResult)Ok(result.Value)
                : (IHttpActionResult)BadRequest(result.Error);

        protected IHttpActionResult MapToHttpResponse(Result result) =>
            result.IsSuccess
                ? (IHttpActionResult)Ok()
                : (IHttpActionResult)BadRequest(result.Error);
    }
}