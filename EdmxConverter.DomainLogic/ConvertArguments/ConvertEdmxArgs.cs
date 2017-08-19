using EdmxConverter.DomainLogic.DataTypes;
using EdmxConverter.DomainLogic.Service;

namespace EdmxConverter.DomainLogic.ConvertArguments
{
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
}