using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema;
using EdmxConv.Schema.DTO;
using Microsoft.AspNetCore.Mvc;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.WebAPI.Controllers
{
    [Route("api/Convert")]
    [Produces("application/json")]
    public class ConvertController : ApiController
    {
        [HttpPost]
        public IActionResult Post([FromBody] ConvertParams payload) =>
            Validate(payload)
                .OnSuccess(p => ConvertEdmxArgsModule.CreateArguments(p))
                .OnSuccess(arg => ConvertModule.Convert(arg))
                .OnBoth(result => MapToHttpResponse(result));

        // Pure
        private static Result<ConvertParams> Validate(ConvertParams payload) =>
            string.IsNullOrWhiteSpace(payload.Edmx) ? Result.Fail<ConvertParams>("EDMX to convert should be not empty.")
            : (payload.Source == payload.Target ? Result.Fail<ConvertParams>("Source and target types are the same.")
            : Result.Ok(payload));
    }


}
