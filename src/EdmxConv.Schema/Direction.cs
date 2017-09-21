using System;
using CSharpFunctionalExtensions;

namespace EdmxConv.Schema
{
    public struct Direction : IEquatable<Direction>
    {
        public EdmxTypeEnum Target { get; }
        public EdmxTypeEnum Source { get; }

        public static Result<Direction> Create(EdmxTypeEnum source, EdmxTypeEnum target) =>
            source == target ? Result.Fail<Direction>("Source and target cannot be the same.")
            : Result.Ok(new Direction(source, target));

        public Direction(EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
        }

        public bool Equals(Direction other) => 
            Target == other.Target && Source == other.Source;

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) && obj is Direction && Equals((Direction)obj);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Target * 397) ^ (int)Source;
            }
        }
    }
}