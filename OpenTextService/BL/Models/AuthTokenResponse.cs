using System.Text.Json.Serialization;

namespace BL.Models
{
    public class AuthTokenResponse
    {
        [JsonPropertyName("refresh_token_expires_in")]
        public string? RefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("api_product_list")]
        public string? ApiProductList { get; set; }

        [JsonPropertyName("api_product_list_json")]
        public List<string>? ApiProductListJson { get; set; }

        [JsonPropertyName("organization_name")]
        public string? OrganizationName { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("issued_at")]
        public string? IssuedAt { get; set; }

        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("application_name")]
        public string? ApplicationName { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public string? ExpiresIn { get; set; }

        [JsonPropertyName("refresh_count")]
        public string? RefreshCount { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
