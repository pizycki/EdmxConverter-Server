namespace EdmxConv.Schema
{
    public abstract class Edmx
    {
        public EdmxTypeEnum Type { get; }

        protected Edmx(EdmxTypeEnum type)
        {
            Type = type;
        }
    }
}