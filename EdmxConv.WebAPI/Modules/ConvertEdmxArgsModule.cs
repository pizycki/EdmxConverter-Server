using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Schema;
using EdmxConv.WebAPI.Controllers;

namespace EdmxConv.WebAPI.Modules
{
    public class ConvertEdmxArgsModule
    {
        public static Result<ConvertEdmxArgs> CreateArguments(ConvertParams @params) =>
            Bla.With(@params)
                .OnSuccess(p => GetEdmxMapper(p.Source).ToResult("Unsupported source Type"))
                .OnSuccess(mapper => mapper(@params.Edmx))
                .OnSuccess(edmx => new ConvertEdmxArgs(edmx, @params.Source, @params.Target));

        private static Maybe<Func<string, Edmx>> GetEdmxMapper(EdmxTypeEnum edmxType)
        {
            try
            {
                return EdmxMappings[edmxType];
            }
            catch (ArgumentOutOfRangeException)
            {
                return new Maybe<Func<string, Edmx>>();
            }
        }

        private static IReadOnlyDictionary<EdmxTypeEnum, Func<string, Edmx>> EdmxMappings { get; } =
            new Dictionary<EdmxTypeEnum, Func<string, Edmx>>
            {
                {EdmxTypeEnum.Xml, s => new XmlEdmx(s)},
                {EdmxTypeEnum.Database, s => new DatabaseEdmx(s)},
                {EdmxTypeEnum.Resource, s => new ResourceEdmx(s)}
            };
    }
}