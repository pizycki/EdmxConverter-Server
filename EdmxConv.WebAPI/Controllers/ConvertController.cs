using System;
using CSharpFunctionalExtensions;
using EdmxConv.Behaviours;
using EdmxConv.Schema;
using EdmxConv.WebAPI.Modules;
using Microsoft.AspNetCore.Mvc;

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
                .OnSuccess(arg => Convert(arg))
                .OnBoth(result => MapToHttpResponse(result));

        private static Result<Edmx> Convert(ConvertEdmxArgs args)
        {
            throw new NotImplementedException();
        }

        // Pure
        private static Result<ConvertParams> Validate(ConvertParams payload) =>
            string.IsNullOrWhiteSpace(payload.Edmx) ? Result.Fail<ConvertParams>("EDMX to convert should be not empty.")
            : (payload.Source == payload.Target ? Result.Fail<ConvertParams>("Source and target types are the same.")
            : Result.Ok(payload));
    }


    public class ConvertParams
    {
        public string Edmx { get; set; }
        public EdmxTypeEnum Source { get; set; }
        public EdmxTypeEnum Target { get; set; }
    }
}
