using System.Web;
using CSharpFunctionalExtensions;
using EdmxConv.Schema.DTO;
using System.Web.Http;
using EdmxConv.Behaviours.Modules;
using EdmxConv.Core;
using EdmxConv.Schema;
using static EdmxConv.Core.FlowHelpers;

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
                .Map(edmx => (plain: edmx.ToString(), type: edmx.Type))
                // This must stay here, because of reference to System.Web
                .OnSuccessIf(condition: edmxWithType => edmxWithType.type == EdmxTypeEnum.Xml,
                             func: edmxWithType =>
                                With(edmxWithType.plain)
                                    .OnSuccess(xml => EncodeHtml(xml))
                                    .Map(xml => (plain: xml, type: EdmxTypeEnum.Xml)))
                .Map(edmxWithType => edmxWithType.plain)
                .OnBoth(MapToHttpResponse);

        private static Result<string> EncodeHtml(string xml) =>
            With(xml)
                .OnSuccess(edmx => edmx.ToString())
                .OnSuccess(edmx => HttpUtility.HtmlEncode(edmx));

        [HttpGet, Route("configuration")]
        public IHttpActionResult GetConfiguration() =>
            Ok(new
            {
                Sources = new dynamic[]
                {
                    new { Name = "XML",      Type = EdmxTypeEnum.Xml },
                    new { Name = "Resource", Type = EdmxTypeEnum.Resource },
                    new { Name = "Database", Type = EdmxTypeEnum.Database }
                },

                Targets = new dynamic[]
                {
                    new { Name = "XML",      Type = EdmxTypeEnum.Xml },
                    new { Name = "Resource", Type = EdmxTypeEnum.Resource },
                    new { Name = "Database", Type = EdmxTypeEnum.Database }
                },
            });

    }
}
