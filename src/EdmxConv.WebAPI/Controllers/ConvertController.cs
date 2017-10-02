﻿using CSharpFunctionalExtensions;
using EdmxConv.Schema.DTO;
using System.Web.Http;
using EdmxConv.Behaviours.Modules;
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
