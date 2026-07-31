using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace SportsFacility.Frontend.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            // Determine the data source mode from appsettings.json
            var mode = _configuration["DataSourceMode"] ?? "Local";

            if (mode.Equals("Api", StringComparison.OrdinalIgnoreCase))
            {
                // Call actual backend authentication API
                try
                {
                    var response = await _httpClient.PostAsJsonAsync("auth/login", new { email, password });
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResult = await response.Content.ReadFromJsonAsync<AuthResult>();
                        return apiResult?.Token ?? string.Empty;
                    }
                    else
                    {
                        // Handle failure (e.g. invalid credentials)
                        return string.Empty; 
                    }
                }
                catch
                {
                    // Handle API connection errors
                    return string.Empty;
                }
            }
            else
            {
                // Local / Mock Mode
                try
                {
                    var mockDataResponse = await _httpClient.GetAsync("mock-auth.json");
                    if (mockDataResponse.IsSuccessStatusCode)
                    {
                        var mockAuthResult = await mockDataResponse.Content.ReadFromJsonAsync<AuthResult>();
                        return mockAuthResult?.Token ?? string.Empty;
                    }
                }
                catch
                {
                    // Fallback if file doesn't exist
                }
                
                return "mock-jwt-token"; // Default local fallback
            }
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
        
        private class AuthResult
        {
            [JsonPropertyName("token")]
            public string Token { get; set; }
        }
    }
}
