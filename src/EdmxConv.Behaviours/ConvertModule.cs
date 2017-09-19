using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public static class ConvertModule
    {
        public static Result<Edmx> Convert(ConvertEdmxArgs args) =>
            With(args)
                .OnSuccess(a => Direction.Create(args.Source, args.Target))
                .OnSuccess(d => Converters.GetMaybe(d).ToResult("Unsupported convertion."))
                .OnSuccess(conv => conv(args.Model));

        private static readonly IReadOnlyDictionary<Direction, Func<Edmx, Result<Edmx>>> Converters =
            new Dictionary<Direction, Func<Edmx, Result<Edmx>>>
            {
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Resource), edmx => ConvertToResource(edmx as DatabaseEdmx) },
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Xml),      edmx => ConvertToXml(edmx as DatabaseEdmx)},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Database), edmx => ConvertToDatabase(edmx as XmlEdmx)},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Resource), edmx => ConvertToResource(edmx as XmlEdmx)},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Xml),      edmx => ConvertToXml(edmx as ResourceEdmx)},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Database), edmx => ConvertToDatabase(edmx as ResourceEdmx)},
            };

        internal static Result<Edmx> ConvertToDatabase(ResourceEdmx xmlEdmx) =>
            Base64Module.Base64ToByteArray(xmlEdmx)
                .OnSuccess(edmx => MiscModule.BytesToHex(edmx))
                .Map(edmx => edmx.ToDatabaseEdmx())
                .Map(edmx => edmx as Edmx);

        internal static Result<Edmx> ConvertToDatabase(XmlEdmx xmlEdmx) =>
            ConvertToResource(xmlEdmx)
                .Map(edmx => edmx as ResourceEdmx)
                .OnSuccess(edmx => ConvertToDatabase(edmx));

        internal static Result<Edmx> ConvertToXml(ResourceEdmx databaseEdmx) =>
            MiscModule.Base64ToGzip(databaseEdmx)
                .OnSuccess(edmx => MiscModule.GZipToXml(edmx))
                .Map(edmx => edmx as Edmx);

        internal static Result<Edmx> ConvertToXml(DatabaseEdmx databaseEdmx) =>
            ConvertToResource(databaseEdmx)
                .Map(edmx => edmx as ResourceEdmx)
                .OnSuccess(edmx => ConvertToXml(edmx));

        internal static Result<Edmx> ConvertToResource(DatabaseEdmx databaseEdmx) =>
            MiscModule.HexToBase64(databaseEdmx)
                .Map(x => x as Edmx);

        internal static Result<Edmx> ConvertToResource(XmlEdmx xmlEdmx) =>
            MiscModule.XmlToGZip(xmlEdmx)
                .OnSuccess(x => MiscModule.GZipToBase64(x))
                .Map(x => x as Edmx);
    }
}
