using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SportsFacility.Frontend.Models;

namespace SportsFacility.Frontend.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        public DashboardService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var mode = _configuration["DataSourceMode"] ?? "Local";

            if (mode.Equals("Api", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    return await _httpClient.GetFromJsonAsync<DashboardDto>("dashboard");
                }
                catch
                {
                    return new DashboardDto();
                }
            }
            else
            {
                try
                {
                    var response =  await _httpClient.GetAsync("mock-dashboard.json");
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<DashboardDto>();
                    }
                }
                catch
                {
                    // Fallback
                }
                
                return new DashboardDto();
            }
        }
    }
}
