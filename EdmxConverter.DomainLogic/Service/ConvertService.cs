using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    public static class ConvertEdmx
    {
        private static Map<Direction, Func<Edmx, Edmx>> Converters { get; } =
            Prelude.Map(
                Prelude.Tuple<Direction, Func<Edmx, Edmx>>(Directions.XmlToResource, ConvertToResource.Convert),
                Prelude.Tuple<Direction, Func<Edmx, Edmx>>(Directions.ResourceToXml, ConvertToXml.Convert));

        public static Edmx Convert(Option<ConvertEdmxArguments> args)
        {
            var direction = MapDirection(args);
            var source = MapSource(args);

            var target = Converters.Find(direction)
                                   .Match(Some: converter => converter(source),
                                          None: () => throw new IndexOutOfRangeException("Converter for that direction is unavailable."));
            return target;
        }

        private static Edmx MapSource(Option<ConvertEdmxArguments> args)
        {
            return args.Map(arg => arg.Source)
                       .IfNone(() => throw new ArgumentException("Source is empty."));
        }

        private static Direction MapDirection(Option<ConvertEdmxArguments> args)
        {
            return args.Map(arg => new Direction(arg.SourceType, arg.TargetType))
                       .IfNone(() => throw new ArgumentException("Cannot build proper direction for convertion."));
        }
    }

    public static class Directions
    {
        public static readonly Direction XmlToResource = new Direction(EdmxTypeEnum.Xml, EdmxTypeEnum.Resource);
        public static readonly Direction ResourceToXml = new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Resource);
    }

    public class Direction
    {
        public Direction(EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
        }

        public EdmxTypeEnum Source { get; set; }
        public EdmxTypeEnum Target { get; set; }
    }

    public enum EdmxTypeEnum
    {
        Xml, Resource, Database
    }

    public static class ConvertToXml
    {
        public static XmlEdmx Convert<TSource>(TSource source) where TSource : Edmx
        {
            // TODO Pattern matching for generic since C#7.1
            //https://github.com/dotnet/csharplang/blob/master/proposals/generics-pattern-match.md

            var edmx = source as ResourceEdmx;
            if (edmx != null)
                return ConvertResourceToXml(edmx);

            var databaseEdmx = source as DatabaseEdmx;
            if (databaseEdmx != null)
                return ConvertDatabaseModelToXml(databaseEdmx);

            throw new ArgumentOutOfRangeException();
        }

        private static XmlEdmx ConvertResourceToXml(ResourceEdmx source)
        {
            return source
                .Then(Service.Convert.Base64ToGZipBinary)
                .Then(Service.Convert.GZipBinaryToPlainXml);
        }

        private static XmlEdmx ConvertDatabaseModelToXml(DatabaseEdmx source)
        {
            throw new NotImplementedException();
        }
    }

    public static class ConvertToResource
    {
        public static ResourceEdmx Convert(Edmx source)
        {
            var xmlEdmx = source as XmlEdmx;
            if (xmlEdmx != null)
                return ConvertFromXml(xmlEdmx);

            var databaseEdmx = source as DatabaseEdmx;
            if (databaseEdmx != null)
                return ConvertFromDatabase(databaseEdmx);

            throw new ArgumentOutOfRangeException();
        }

        private static ResourceEdmx ConvertFromDatabase(DatabaseEdmx databaseEdmx)
        {
            throw new NotImplementedException();
        }

        private static ResourceEdmx ConvertFromXml(XmlEdmx xmlEdmx)
        {
            return xmlEdmx
                .Then(Service.Convert.XmlEdmxToByteArray)
                .Then(Service.Convert.BytesToGzipBinary)
                .Then(Service.Convert.GZipBinaryToBase64);
        }

    }

    public static class Convert
    {
        public static GZipBinary Base64ToGZipBinary(ResourceEdmx source) => new GZipBinary(System.Convert.FromBase64String(source.Value));

        public static XmlEdmx GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.Value))
            using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip))
            {
                return XmlEdmx.Create(reader.ReadToEnd());
            }
        }

        public static GZipBinary BytesToGzipBinary(byte[] source)
        {
            using (var resultStream = new MemoryStream(source.Length))
            using (var compressionStream = new GZipStream(resultStream, CompressionMode.Compress))
            {
                compressionStream.Write(source, 0, source.Length);
                return new GZipBinary(resultStream.ToArray());
            }
        }

        public static ResourceEdmx GZipBinaryToBase64(GZipBinary source) =>
            System.Convert.ToBase64String(source.Value)
            .Then(v => ResourceEdmx.Create(v));

        public static byte[] XmlEdmxToByteArray(XmlEdmx arg) => Encoding.ASCII.GetBytes(arg.Value);
    }

    public static class FlowExtensions
    {
        public static TResult Then<TObject, TResult>(this TObject obj, Func<TObject, TResult> func)
        {
            return func(obj);
        }
    }

    public class GZipBinary : Edmx
    {
        public byte[] Value { get; }
        public EdmxType Type { get; }

        public GZipBinary(Option<byte[]> value) =>
            Value = value.IfNone(new byte[0]);
    }

    public class XmlEdmx : Edmx
    {
        public string Value { get; }
        public EdmxType Type { get; } = XmlEdmxType.Create();

        private XmlEdmx(Option<string> value) => Value = value.IfNone(string.Empty);
        public static XmlEdmx Create(Option<string> value) => new XmlEdmx(value);

    }

    public class ResourceEdmx : Edmx
    {
        public string Value { get; }
        public EdmxType Type { get; } = ResourceEdmxType.Create();

        public static ResourceEdmx Create(Option<string> value) => new ResourceEdmx(value);
        private ResourceEdmx(Option<string> value) => Value = value.IfNone(string.Empty);
    }

    public class DatabaseEdmx : Edmx
    {
        public string Value { get; }
        public EdmxType Type { get; } = DatabaseEdmxType.Create();

        public static DatabaseEdmx Create(Option<string> value) => new DatabaseEdmx(value);
        private DatabaseEdmx(Option<string> value) => Value = value.IfNone(string.Empty);
    }

    public abstract class Edmx
    {
        public EdmxType Type { get; }
    }

    public abstract class EdmxType { }

    public class XmlEdmxType : EdmxType
    {
        private XmlEdmxType() { }
        public static XmlEdmxType Create() => new XmlEdmxType();
    }
    public class ResourceEdmxType : EdmxType
    {
        private ResourceEdmxType() { }
        public static ResourceEdmxType Create() => new ResourceEdmxType();
    }
    public class DatabaseEdmxType : EdmxType
    {
        private DatabaseEdmxType() { }
        public static DatabaseEdmxType Create() => new DatabaseEdmxType();
    }

    public class ConvertEdmxArguments
    {
        public EdmxTypeEnum SourceType { get; set; }
        public EdmxTypeEnum TargetType { get; set; }

        public Edmx Source { get; set; }
    }

}