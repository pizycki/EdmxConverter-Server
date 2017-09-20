using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.DTO;
using EdmxConv.Schema.Extensions;
using static EdmxConv.Core.FlowHelpers;
namespace EdmxConv.Behaviours
{
    public class ConvertEdmxArgsModule
    {
        public static Result<ConvertEdmxArgs> CreateArguments(ConvertParams convParams) =>
            With(convParams)
                .OnSuccess(GetEdmxMapper())
                .OnSuccess(MapEdmx(convParams))
                .OnSuccess(Convert(convParams));

        private static Func<Edmx, ConvertEdmxArgs> Convert(ConvertParams @params) =>
            edmx => new ConvertEdmxArgs(edmx, @params.Source, @params.Target);

        private static Func<Func<string, (Error, Edmx)>, Result<Edmx>> MapEdmx(ConvertParams @params) =>
            edmxMapper =>
            {
                var edmx = edmxMapper(@params.Edmx);
                return edmx.Item1 != string.Empty // fst value is error
                    ? Result.Fail<Edmx>(edmx.Item1)
                    : Result.Ok(edmx.Item2);
            };

        private static Func<ConvertParams, Result<Func<string, (string, Edmx)>>> GetEdmxMapper() =>
            p => EdmxMappings.GetMaybe(p.Source).ToResult("Unsupported source Type");

        private static IReadOnlyDictionary<EdmxTypeEnum, Func<string, (string, Edmx)>> EdmxMappings { get; } =
            new Dictionary<EdmxTypeEnum, Func<string, (string, Edmx)>>
            {
                { EdmxTypeEnum.Xml,      s => GetEdmxOrError(s.ToXmlEdmx)},
                { EdmxTypeEnum.Database, s => GetEdmxOrError(s.ToDatabaseEdmx)},
                { EdmxTypeEnum.Resource, s => GetEdmxOrError(s.ToResourceEdmx) }
            };

        private static (string, Edmx) GetEdmxOrError<T>(Func<Result<T>> func) where T : Edmx
        {
            var result = func().OnSuccess(edmx => edmx.GetAs<Edmx>());
            return result.IsSuccess ? (string.Empty, result.Value) : (result.Error, null as Edmx);
        }
    }
}