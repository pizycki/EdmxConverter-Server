using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.DTO;
using EdmxConv.Schema.Extensions;

namespace EdmxConv.Behaviours.Modules
{
    public class ConvertEdmxArgsModule
    {
        public static Result<ConvertEdmxArgs> CreateArguments(ConvertParams convParams) =>
            FlowHelpers.With(convParams)
                .OnSuccess(GetEdmxMapper())
                .OnSuccess(MapEdmx(convParams))
                .OnSuccess(Convert(convParams));

        private static Func<Edmx, ConvertEdmxArgs> Convert(ConvertParams @params) =>
            edmx => new ConvertEdmxArgs(edmx, @params.Source, @params.Target);

        private static Func<Func<string, (Error error, Edmx edmx)>, Result<Edmx>> MapEdmx(ConvertParams @params) =>
            mapper =>
            {
                var edmxOrError = mapper(@params.Edmx);
                return edmxOrError.error != Error.Empty
                    ? Result.Fail<Edmx>(edmxOrError.error.Message)
                    : Result.Ok(edmxOrError.edmx);
            };

        private static Func<ConvertParams, Result<Func<string, (Error, Edmx)>>> GetEdmxMapper() =>
            p => EdmxMappings.GetMaybe(p.Source)
                             .ToResult("Unsupported source Type");

        private static IReadOnlyDictionary<EdmxTypeEnum, Func<string, (Error, Edmx)>> EdmxMappings { get; } =
            new Dictionary<EdmxTypeEnum, Func<string, (Error, Edmx)>>
            {
                { EdmxTypeEnum.Xml,      s => GetEdmxOrError(s.ToXmlEdmx)},
                { EdmxTypeEnum.Database, s => GetEdmxOrError(s.ToDatabaseEdmx)},
                { EdmxTypeEnum.Resource, s => GetEdmxOrError(s.ToResourceEdmx) }
            };

        private static (Error, Edmx) GetEdmxOrError<T>(Func<Result<T>> func) where T : Edmx
        {
            var result = func().OnSuccess(edmx => edmx.GetAs<Edmx>());
            return result.IsSuccess ? (Error.Empty, result.Value) : (Error.Create(result.Error), null as Edmx);
        }
    }
}