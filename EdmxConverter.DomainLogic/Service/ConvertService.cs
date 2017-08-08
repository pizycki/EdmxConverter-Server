using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    public static class ConvertEdmx
    {
        public static Edmx Convert(Option<ConvertEdmxArguments> args)
        {
            var direction = MapDirection(args);
            var source = MapSource(args);

            // Xml -> Resource ======================================

            if (direction == Directions.XmlToResource)
                return ConvertToResource.Convert(source)
                                          .IfNone(new ResourceEdmx(string.Empty));


            // Resource -> Xml ======================================

            if (direction == Directions.ResourceToXml)
                return ConvertToXml.Convert(source)
                                     .IfNone(new XmlEdmx(string.Empty));


            throw new ArgumentOutOfRangeException();
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

        public EdmxTypeEnum Source { get; }
        public EdmxTypeEnum Target { get; }
    }

    public enum EdmxTypeEnum
    {
        Xml, Resource, Database
    }

    public static class ConvertToXml
    {
        public static Option<XmlEdmx> Convert(Edmx source)
        {
            switch (source)
            {
                case ResourceEdmx res:
                    return ConvertResourceToXml(res);

                case DatabaseEdmx db:
                    return ConvertDatabaseModelToXml(db);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Option<XmlEdmx> ConvertResourceToXml(ResourceEdmx source)
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
        public static Option<ResourceEdmx> Convert(Edmx source)
        {
            switch (source)
            {
                case XmlEdmx xml:
                    return ConvertFromXml(xml);

                case DatabaseEdmx db:
                    return ConvertFromDatabase(db);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static ResourceEdmx ConvertFromDatabase(DatabaseEdmx databaseEdmx)
        {
            throw new NotImplementedException();
        }

        private static ResourceEdmx ConvertFromXml(XmlEdmx xmlEdmx)
        {
            return xmlEdmx
                .Then(Service.Convert.XmlEdmxToGzipBinary)
                .Then(Service.Convert.GZipBinaryToBase64);
        }
    }

    public static class Convert
    {
        public static GZipBinary Base64ToGZipBinary(ResourceEdmx source) => new GZipBinary(System.Convert.FromBase64String(source.Value));

        public static Option<XmlEdmx> GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.Value))
            using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip))
            {
                return Some(new XmlEdmx(reader.ReadToEnd()));
            }
        }

        public static GZipBinary XmlEdmxToGzipBinary(XmlEdmx xmlEdmx)
        {
            using (var outStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                {
                    xmlEdmx.Value.Save(gzipStream);
                }

                return new GZipBinary(outStream.ToArray());
            }
        }

        public static ResourceEdmx GZipBinaryToBase64(GZipBinary source) =>
            System.Convert.ToBase64String(source.Value)
            .Then(v => new ResourceEdmx(v));
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

        public GZipBinary(Option<byte[]> value) => Value = value.IfNone(new byte[0]);
    }


    public class XmlEdmx : Edmx
    {
        public XDocument Value { get; }
        public EdmxType Type { get; } = XmlEdmxType.Create();

        public XmlEdmx(Option<string> plainXml)
        {
            Value = plainXml.Match(
                Some: XDocument.Parse,
                None: () => throw new ArgumentNullException(nameof(plainXml)));
        }

        public XmlEdmx(Option<XDocument> document)
        {
            Value = document.Match(
                Some: Validate,
                None: () => throw new ArgumentNullException(nameof(document)));
        }

        private XDocument Validate(XDocument doc)
        {
            // TODO validation

            return doc;
        }

        public override string ToString()
        {
            return Value.ToString(SaveOptions.OmitDuplicateNamespaces);
        }
    }

    public class ResourceEdmx : Edmx
    {
        public string Value { get; }
        public EdmxType Type { get; } = ResourceEdmxType.Create();

        public ResourceEdmx(Option<string> value) => Value = value.IfNone(string.Empty);
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
        public EdmxTypeEnum Type { get; }
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