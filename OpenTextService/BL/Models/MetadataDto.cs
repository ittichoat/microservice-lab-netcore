namespace BL.Models
{
    public class MetadataDto
    {
        public int CategoryId { get; set; }
        public Dictionary<string, object> Attributes { get; set; } = new();
    }
}
