using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;

using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public static class ConvertModule
    {
        public static Result<Edmx> Convert(ConvertEdmxArgs args) =>
            With(args)
                .OnSuccess(a => Direction.Create(args.Source, args.Target))
                .OnSuccess(d => Converters.GetMaybe(d).ToResult("Unsupported convertion."))
                .OnSuccess(conv => conv(args));


        public static readonly IReadOnlyDictionary<Direction, Func<ConvertEdmxArgs, Edmx>> Converters =
            new Dictionary<Direction, Func<ConvertEdmxArgs, Edmx>>
            {
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Resource), null},
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Xml),      null},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Database), null},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Resource), null},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Xml),      null},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Database), null},
            };

    }
}
