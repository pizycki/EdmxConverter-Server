using CSharpFunctionalExtensions;
using EdmxConv.Schema.Extensions;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Schema
{
    public sealed class ResourceEdmx : Edmx
    {
        /// <summary>
        /// BASE64 value
        /// </summary>
        /// <example>H4sIAAAAAAAEAM1X227bOBB9X2D/g ... XQ0D7gMAAA=</example>
        public string Value { get; }

        public ResourceEdmx(string value)
            : base(EdmxTypeEnum.Resource) =>
            Value = value;

        public override string ToString() => Value;

        public static Result<ResourceEdmx> Create(string edmx) =>
            With(edmx)
                .OnSuccess(x => x.ToResourceEdmx());
    }
}