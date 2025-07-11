using BL.Models;

namespace BL.Services
{
    public interface IDocumentService
    {
        Task<Document?> GetDocumentAsync(int id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<bool> UploadDocumentAsync(Document doc);
        Task<bool> DeleteDocumentAsync(int id);
    }
}
