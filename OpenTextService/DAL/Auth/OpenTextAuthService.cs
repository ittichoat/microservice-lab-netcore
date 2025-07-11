using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DAL.Auth
{
    public class OpenTextAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenTextAuthService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = _config["OpenText:TokenUrl"];
            var clientId = _config["OpenText:ClientId"];
            var clientSecret = _config["OpenText:ClientSecret"];
            var scope = _config["OpenText:Scope"];

            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
            {
                Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", clientId!),
                    new KeyValuePair<string, string>("client_secret", clientSecret!),
                    new KeyValuePair<string, string>("scope", scope!)
                })
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<OAuthTokenResponse>(json);

            return tokenResponse?.AccessToken ?? throw new Exception("No access token received.");
        }

        private class OAuthTokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; } = string.Empty;

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
        }
    }
}
