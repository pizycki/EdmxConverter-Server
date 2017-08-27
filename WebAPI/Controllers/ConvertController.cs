using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using EdmxConverter.DomainLogic.Converting;
using EdmxConverter.DomainLogic.Converting.Arguments;
using EdmxConverter.DomainLogic.Types;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

using static LanguageExt.Prelude;
using static EdmxConverter.WebAPI.Controllers.DirectionBuilder;
using static EdmxConverter.DomainLogic.Types.EdmxTypeEnum;

namespace EdmxConverter.WebAPI.Controllers
{
    [Route("api/convert")]
    public class ConvertController : Controller
    {
        [HttpPost, Route("")]
        public IActionResult Post([FromBody]ConvertParams parameters)
        {
            Option<ConvertEdmxArgs> CreateArguments((Edmx edmx, EdmxTypeEnum target) edmxWithTarget)
            {
                switch (edmxWithTarget.edmx)
                {
                    case XmlEdmx xmlEdmx:
                        return edmxWithTarget.target == Database
                            ? new XmlToDatabaseArgs(xmlEdmx) as ConvertEdmxArgs
                            : new XmlToResourceArgs(xmlEdmx) as ConvertEdmxArgs;

                    case DatabaseEdmx dbEdmx:
                        return edmxWithTarget.target == Resource
                            ? new DatabaseToResourceArgs(dbEdmx) as ConvertEdmxArgs
                            : new DatabaseToXmlArgs(dbEdmx) as ConvertEdmxArgs;

                    case ResourceEdmx resEdmx:
                        return edmxWithTarget.target == Xml
                            ? new ResourceToXmlArgs(resEdmx) as ConvertEdmxArgs
                            : new ResourceToDatabaseArgs(resEdmx) as ConvertEdmxArgs;

                    default:
                        return Option<ConvertEdmxArgs>.None;
                }
            }

            Option<string> ConvertWithArgs(Func<(Edmx, EdmxTypeEnum), Option<ConvertEdmxArgs>> createArgs,
                (Edmx, EdmxTypeEnum) edmxWithTarget) =>
                createArgs(edmxWithTarget).Match(ConvertEdmx.Convert, () => None);

            var argDict = new Dictionary<Direction, Func<string, Edmx>>
            {
                { Direction(Xml, Database), s => new XmlEdmx(s) },
                { Direction(Xml, Resource), s => new XmlEdmx(s) },

                { Direction(Resource, Database), s => new ResourceEdmx(s) },
                { Direction(Resource, Xml), s => new ResourceEdmx(s) },

                { Direction(Database , Resource), s => new DatabaseEdmx(s) },
                { Direction(Database, Xml), s => new DatabaseEdmx(s) }
            };

            var @params = Some(parameters);
            var result = from source in @params.Map(x => x.Edmx)
                         from target in @params.Map(x => x.Target)
                         from create in @params.Map(p => Direction(p.Source, p.Target))
                                               .Map(key => argDict[key])
                         let edmx = create(source)
                         let edmxWithTarget = (edmx, target)
                         let resultEdmx = ConvertWithArgs(CreateArguments, edmxWithTarget)
                         select resultEdmx.Match<HttpActionResult>(
                             Some: x => new Success<string>(x),
                             None: () => new Fail(""));

            return result.Match(ToHttpResponse, BadRequest);
        }

        private IActionResult ToHttpResponse(HttpActionResult httpActionResult)
        {
            switch (httpActionResult)
            {
                case Success succ:
                    return Ok(succ);

                case Fail fail:
                    return MapFailToHttpResponse(fail);

                default:
                    return StatusCode(500);
            }
        }

        private IActionResult MapFailToHttpResponse(Fail fail) => BadRequest(fail);
    }

    public class ConvertParams
    {
        public string Edmx { get; set; }
        public EdmxTypeEnum Source { get; set; }
        public EdmxTypeEnum Target { get; set; }
    }

    public struct Direction
    {
        private EdmxTypeEnum _source;
        private EdmxTypeEnum _target;

        public Direction(EdmxTypeEnum source, EdmxTypeEnum target)
        {
            _source = source;
            _target = target;
        }
    }

    public static class DirectionBuilder
    {
        public static Direction Direction(EdmxTypeEnum source, EdmxTypeEnum target) => new Direction(source, target);
    }

    public class HttpActionResult
    {
        public HttpStatusCode StatusCode { get; }

        protected HttpActionResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }

    public class Success : HttpActionResult
    {
        protected Success() : base(HttpStatusCode.OK)
        {
        }
    }

    public class Success<T> : Success
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public Option<T> Content { get; } = None;

        public Success()
        {
        }

        public Success(T content)
        {
            Content = Some(content);
        }
    }

    public class Fail : HttpActionResult
    {
        public string Error { get; }

        public Fail(string error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(statusCode)
        {
            Error = error;
        }
    }
}