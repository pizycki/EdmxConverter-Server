using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema.DTO;
using System.Web.Http;

namespace EdmxConv.WebAPI.Controllers
{
    [RoutePrefix("api/convert")]
    public class ConvertController : BaseApiController
    {
        [HttpPost, Route("")]
        public IHttpActionResult Post([FromBody] ConvertParams payload) =>
            ConvertParamsValidationModule.Validate(payload)
                .OnSuccess(p => ConvertEdmxArgsModule.CreateArguments(p))
                .OnSuccess(arg => ConvertModule.Convert(arg))
                .Map(edmx => edmx.ToString())
                .OnBoth(MapToHttpResponse);
    }
}
