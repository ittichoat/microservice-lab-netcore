using BL.Interfaces;
using BL.Models;
using DAL.Auth;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace DAL.Repositories
{
    public class OpenTextRepository : IOpenTextRepository
    {
        private readonly OpenTextAuthService _authService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenTextRepository(OpenTextAuthService authService, HttpClient httpClient, IConfiguration config)
        {
            _authService = authService;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            var token = await _authService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var baseUrl = _config["OpenText:BaseUrl"];
            var response = await _httpClient.GetAsync($"{baseUrl}/content/api/v1/nodes/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            // Map JSON to Document model here (pseudo)
            return new Document { Id = id, Name = "ExampleDoc" };
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync() => throw new NotImplementedException();
        public async Task<bool> UploadDocumentAsync(Document doc) => throw new NotImplementedException();
        public async Task<bool> DeleteDocumentAsync(int id) => throw new NotImplementedException();
    }
}
