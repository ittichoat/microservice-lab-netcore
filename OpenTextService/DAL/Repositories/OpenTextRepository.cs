using BL.Interfaces;
using BL.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace DAL.Repositories
{
    public class OpenTextRepository : IOpenTextRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenTextRepository(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<AuthTokenResponse?> GetAccessTokenAsync(AuthRequest form)
        {
            var url = $"https://{form.Region}.api.opentext.com/tenants/{form.TenantId}/oauth2/token";

            var dic = new Dictionary<string, string>
            {
                ["client_id"] = form.ClientId,
                ["client_secret"] = form.ClientSecret,
                ["grant_type"] = "client_credentials"
            };

            using var content = new FormUrlEncodedContent(dic);

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Optional: ช่วยให้ server ตอบ JSON
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed ({response.StatusCode}): {errorBody}");
            }

            return await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
        }

        public async Task<Document?> GetDocumentByIdAsync(int id)
        {
            //var token = await _authService.GetAccessTokenAsync();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var baseUrl = _config["OpenText:BaseUrl"];
            //var response = await _httpClient.GetAsync($"{baseUrl}/content/api/v1/nodes/{id}");
            //response.EnsureSuccessStatusCode();

            //var json = await response.Content.ReadAsStringAsync();
            //// Map JSON to Document model here (pseudo)
            return new Document { Id = id, Name = "ExampleDoc" };
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync() => throw new NotImplementedException();
        public async Task<bool> UploadDocumentAsync(Document doc) => throw new NotImplementedException();
        public async Task<bool> DeleteDocumentAsync(int id) => throw new NotImplementedException();
    }
}
