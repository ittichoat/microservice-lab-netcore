using BL.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BL.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public DocumentService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<int> UploadDocumentAsync(string token, UploadFileRequest form)
        {
            var baseUrl = _config["OpenText:ApiBaseUrl"];
            var url = $"{baseUrl}/v1/nodes";

            using var content = new MultipartFormDataContent();
            using var stream = form.File.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, "file", form.File.FileName);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetInt32();
        }

        public async Task AttachMetadataAsync(string token, int nodeId, MetadataDto metadata)
        {
            var baseUrl = _config["OpenText:ApiBaseUrl"];
            var url = $"{baseUrl}/v1/nodes/{nodeId}/categories/{metadata.CategoryId}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new { attributes = metadata.Attributes };
            var response = await _httpClient.PutAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task<DocumentInfoDto> GetDocumentAsync(string token, int nodeId)
        {
            var baseUrl = _config["OpenText:ApiBaseUrl"];
            var url = $"{baseUrl}/v1/nodes/{nodeId}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<DocumentInfoDto>()!;
        }

        public async Task DeleteDocumentAsync(string token, int nodeId)
        {
            var baseUrl = _config["OpenText:ApiBaseUrl"];
            var url = $"{baseUrl}/v1/nodes/{nodeId}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
