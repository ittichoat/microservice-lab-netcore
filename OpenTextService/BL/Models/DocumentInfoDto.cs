namespace BL.Models
{
    public class DocumentInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreateUser { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
