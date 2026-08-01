using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;

    public JwtAuthorizationHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var isAuthRequest = request.RequestUri != null && 
            (request.RequestUri.AbsolutePath.Contains("auth/login", StringComparison.OrdinalIgnoreCase) || 
             request.RequestUri.AbsolutePath.Contains("auth/refresh", StringComparison.OrdinalIgnoreCase));

        var token = await GetLocalItemAsync("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !isAuthRequest)
        {
            var refreshToken = await GetLocalItemAsync("refreshToken");
            if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(refreshToken))
            {
                var baseAddress = request.RequestUri!.GetLeftPart(UriPartial.Authority);
                var refreshed = await TryRefreshTokenAsync(baseAddress, token, refreshToken);
                if (refreshed != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshed.Token);
                    
                    response.Dispose();
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    await ClearAuthStorageAsync();
                }
            }
        }

        return response;
    }

    private async Task<string?> GetLocalItemAsync(string key)
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }
        catch
        {
            return null;
        }
    }

    private async Task ClearAuthStorageAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
        }
        catch {}
    }

    private async Task<RefreshResult?> TryRefreshTokenAsync(string baseAddress, string token, string refreshToken)
    {
        try
        {
            using var client = new HttpClient();
            var url = $"{baseAddress.TrimEnd('/')}/api/auth/refresh";
            var response = await client.PostAsJsonAsync(url, new { Token = token, RefreshToken = refreshToken });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RefreshResult>();
                if (result != null && !string.IsNullOrWhiteSpace(result.Token))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", result.Token);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", result.RefreshToken);
                    return result;
                }
            }
        }
        catch {}
        return null;
    }

    private class RefreshResult
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}