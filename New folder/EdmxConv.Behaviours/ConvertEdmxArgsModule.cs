using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.DTO;
using static EdmxConv.Core.FlowHelpers;
namespace EdmxConv.Behaviours
{
    public class ConvertEdmxArgsModule
    {
        public static Result<ConvertEdmxArgs> CreateArguments(ConvertParams convParams) =>
            With(convParams)
                .OnSuccess(GetEdmxMapper())
                .OnSuccess(Map(convParams))
                .OnSuccess(Convert(convParams));

        private static Func<Edmx, ConvertEdmxArgs> Convert(ConvertParams @params) =>
            edmx => new ConvertEdmxArgs(edmx, @params.Source, @params.Target);

        private static Func<Func<string, Edmx>, Edmx> Map(ConvertParams @params) =>
            mapper => mapper(@params.Edmx);

        private static Func<ConvertParams, Result<Func<string, Edmx>>> GetEdmxMapper() =>
            p => EdmxMappings.GetMaybe(p.Source).ToResult("Unsupported source Type");

        private static IReadOnlyDictionary<EdmxTypeEnum, Func<string, Edmx>> EdmxMappings { get; } =
            new Dictionary<EdmxTypeEnum, Func<string, Edmx>>
            {
                { EdmxTypeEnum.Xml,      s => new XmlEdmx(s)},
                { EdmxTypeEnum.Database, s => new DatabaseEdmx(s)},
                { EdmxTypeEnum.Resource, s => new ResourceEdmx(s)}
            };
    }
}