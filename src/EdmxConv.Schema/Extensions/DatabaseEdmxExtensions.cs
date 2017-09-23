namespace EdmxConv.Schema.Extensions
{
    public static class DatabaseEdmxExtensions
    {
        public static DatabaseEdmx ToDatabaseEdmx(this Hex hex) => new DatabaseEdmx(hex);
    }
}