namespace EdmxConv.Schema.DTO
{
    public class ConvertParams
    {
        public string Edmx { get; set; }
        public EdmxTypeEnum Source { get; set; }
        public EdmxTypeEnum Target { get; set; }
    }
}
