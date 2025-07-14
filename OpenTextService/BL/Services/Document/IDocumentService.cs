using BL.Models;

namespace BL.Services
{
    public interface IDocumentService
    {
        Task<int> UploadDocumentAsync(string token, UploadFileRequest form);
        Task AttachMetadataAsync(string token, int nodeId, MetadataDto metadata);
        Task<DocumentInfoDto> GetDocumentAsync(string token, int nodeId);
        Task DeleteDocumentAsync(string token, int nodeId);
    }
}
