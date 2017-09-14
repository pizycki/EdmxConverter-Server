using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EdmxConv.WebAPI.Controllers
{
    [Route("api/Convert")]
    [Produces("application/json")]
    public class ConvertController : ApiController
    {
        [HttpPost, Route("")]
        public IActionResult Post([FromBody] ConvertParams payload) =>
            Validate(payload)
                .OnSuccess(p => ConvertEdmxArgsModule.CreateArguments(p))
                .OnSuccess(arg => ConvertModule.Convert(arg))
                .Map(edmx => edmx.ToString())
                .OnBoth(MapToHttpResponse);

        // Pure
        private static Result<ConvertParams> Validate(ConvertParams payload) =>
            string.IsNullOrWhiteSpace(payload.Edmx) ? Result.Fail<ConvertParams>("EDMX to convert should be not empty.")
            : (payload.Source == payload.Target ? Result.Fail<ConvertParams>("Source and target types are the same.")
            : Result.Ok(payload));
    }


}
