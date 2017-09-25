using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema.DTO;
using System.Web.Http;
using EdmxConv.Schema;

namespace EdmxConv.WebAPI.Controllers
{
    [RoutePrefix("api/convert")]
    public class ConvertController : BaseApiController
    {
        [HttpPost, Route("")]
        public IHttpActionResult Convert([FromBody] ConvertParams payload) =>
            ConvertParamsValidationModule.Validate(payload)
                .OnSuccess(p => ConvertEdmxArgsModule.CreateArguments(p))
                .OnSuccess(arg => ConvertModule.Convert(arg))
                .Map(edmx => edmx.ToString())
                .OnBoth(MapToHttpResponse);

        [HttpGet, Route("targets")]
        public IHttpActionResult Targets() => Ok(new dynamic[]
        {
            new { Name = "XML",      Type = EdmxTypeEnum.Xml },
            new { Name = "Resource", Type = EdmxTypeEnum.Resource },
            new { Name = "Database", Type = EdmxTypeEnum.Database }
        });

        [HttpGet, Route("sources")]
        public IHttpActionResult Sources() => Ok(new dynamic[]
        {
            new { Name = "XML",      Type = EdmxTypeEnum.Xml },
            new { Name = "Resource", Type = EdmxTypeEnum.Resource },
            new { Name = "Database", Type = EdmxTypeEnum.Database }
        });

    }
}
