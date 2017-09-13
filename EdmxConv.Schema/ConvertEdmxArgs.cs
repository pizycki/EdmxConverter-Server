namespace EdmxConv.Schema
{
    public class ConvertEdmxArgs
    {
        public ConvertEdmxArgs(Edmx model, EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
            Model = model;
        }

        public EdmxTypeEnum Source { get; }
        public EdmxTypeEnum Target { get; }
        private Edmx Model { get; }
    }
}
