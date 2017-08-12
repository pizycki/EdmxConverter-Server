namespace EdmxConverter.DomainLogic.Service
{
    // ConvertEdmxArgs

    public abstract class ConvertEdmxArgs
    {
        protected ConvertEdmxArgs(Edmx model, EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
            Model = model;
        }

        public EdmxTypeEnum Source { get; }
        public EdmxTypeEnum Target { get; }
        private Edmx Model { get; }
    }


    // ConvertFrom.....

    public abstract class ConvertFromXmlArgs : ConvertEdmxArgs
    {
        protected ConvertFromXmlArgs(XmlEdmx source, EdmxTypeEnum target) : base(source, EdmxTypeEnum.Xml, target)
        {
            Model = source;
        }

        public XmlEdmx Model { get; }
    }

    public abstract class ConvertFromResourceArgs : ConvertEdmxArgs
    {
        protected ConvertFromResourceArgs(ResourceEdmx source, EdmxTypeEnum target) : base(source, EdmxTypeEnum.Resource, target)
        {
            Model = source;
        }

        public ResourceEdmx Model { get; }
    }

    public abstract class ConvertFromDatabaseArgs : ConvertEdmxArgs
    {
        protected ConvertFromDatabaseArgs(DatabaseEdmx source, EdmxTypeEnum target) : base(source, EdmxTypeEnum.Database, target)
        {
            Model = source;
        }

        public DatabaseEdmx Model { get; }
    }





    // Convert .... To ....

    // From FromResource

    public sealed class ConvertResourceToXml : ConvertFromResourceArgs
    {
        public ConvertResourceToXml(ResourceEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    public sealed class ConvertResourceToDatabase : ConvertFromResourceArgs
    {
        public ConvertResourceToDatabase(ResourceEdmx source) : base(source, EdmxTypeEnum.Database) { }
    }

    // From Database

    public sealed class ConvertDatabaseToResource : ConvertFromDatabaseArgs
    {
        public ConvertDatabaseToResource(DatabaseEdmx source) : base(source, EdmxTypeEnum.Resource) { }
    }

    public sealed class ConvertDatabaseToXml : ConvertFromDatabaseArgs
    {
        public ConvertDatabaseToXml(DatabaseEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    // From XML

    public sealed class ConvertXmlToDatabase : ConvertFromXmlArgs
    {
        public ConvertXmlToDatabase(XmlEdmx source) : base(source, EdmxTypeEnum.Xml) { }
    }

    public class ConvertXmlToResource : ConvertFromXmlArgs
    {
        public ConvertXmlToResource(XmlEdmx source) : base(source, EdmxTypeEnum.Resource) { }
    }
}
