using System;

namespace EdmxConv.Schema
{
    public struct ConvertEdmxArgs : IEquatable<ConvertEdmxArgs>
    {
        public EdmxTypeEnum Source { get; }
        public EdmxTypeEnum Target { get; }
        public Edmx Model { get; }

        public ConvertEdmxArgs(Edmx model, EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
            Model = model;
        }

        public bool Equals(ConvertEdmxArgs other) => Source == other.Source && Target == other.Target && Equals(Model, other.Model);

        public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (obj is ConvertEdmxArgs && Equals((ConvertEdmxArgs)obj));

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Source;
                hashCode = (hashCode * 397) ^ (int)Target;
                hashCode = (hashCode * 397) ^ (Model != null ? Model.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
