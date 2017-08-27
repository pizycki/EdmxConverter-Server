using EdmxConverter.Schema;

namespace EdmxConverter.Logic.Arguments
{
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
}
