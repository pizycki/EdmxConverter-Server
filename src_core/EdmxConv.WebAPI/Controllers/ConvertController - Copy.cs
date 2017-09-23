using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EdmxConv.WebAPI.Controllers
{
    [Route("api/convert")]
    [Produces("application/json")]
    public class ConvertController : ApiController
    {
        [HttpPost, Route("")]
        public IActionResult Post([FromBody] ConvertParams payload) =>
            ConvertParamsValidationModule.Validate(payload)
                .OnSuccess(p => ConvertEdmxArgsModule.CreateArguments(p))
                .OnSuccess(arg => ConvertModule.Convert(arg))
                .Map(edmx => edmx.ToString())
                .OnBoth(MapToHttpResponse);
    }
}
