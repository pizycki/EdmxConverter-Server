using EdmxConverter.DomainLogic.Types;

namespace EdmxConverter.DomainLogic.Converting.Arguments
{
    // From FromResource

    public sealed class ResourceToXmlArgs : ConvertFromResourceArgs
    {
        public ResourceToXmlArgs(ResourceEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    public sealed class ResourceToDatabaseArgs : ConvertFromResourceArgs
    {
        public ResourceToDatabaseArgs(ResourceEdmx source) : base(source, EdmxTypeEnum.Database) { }
    }

    // From Database

    public sealed class DatabaseToResourceArgs : ConvertFromDatabaseArgs
    {
        public DatabaseToResourceArgs(DatabaseEdmx source) : base(source, EdmxTypeEnum.Resource) { }
    }

    public sealed class DatabaseToXmlArgs : ConvertFromDatabaseArgs
    {
        public DatabaseToXmlArgs(DatabaseEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    // From XML

    public sealed class XmlToDatabaseArgs : ConvertFromXmlArgs
    {
        public XmlToDatabaseArgs(XmlEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    public class XmlToResourceArgs : ConvertFromXmlArgs
    {
        public XmlToResourceArgs(XmlEdmx source) : base(source, EdmxTypeEnum.Resource) { }
    }
}
