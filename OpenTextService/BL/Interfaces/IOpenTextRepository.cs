﻿

using BL.Models;

namespace BL.Interfaces
{
    public interface IOpenTextRepository
    {
        Task<AuthTokenResponse?> GetAccessTokenAsync(AuthRequest form);
        Task<Document?> GetDocumentByIdAsync(int id);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<bool> UploadDocumentAsync(Document doc);
        Task<bool> DeleteDocumentAsync(int id);
    }
}
