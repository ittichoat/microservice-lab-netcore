using BL.Interfaces;
using BL.Models;

namespace BL.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IOpenTextRepository _repo;

        public DocumentService(IOpenTextRepository repo)
        {
            _repo = repo;
        }

        public Task<Document?> GetDocumentAsync(int id)
        {
            return _repo.GetDocumentByIdAsync(id);
        }

        public Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return _repo.GetAllDocumentsAsync();
        }

        public Task<bool> UploadDocumentAsync(Document doc)
        {
            return _repo.UploadDocumentAsync(doc);
        }

        public Task<bool> DeleteDocumentAsync(int id)
        {
            return _repo.DeleteDocumentAsync(id);
        }
    }
}
