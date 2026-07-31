using Microsoft.JSInterop;
using System.Net.Http.Headers;

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
        var token = await _jsRuntime.InvokeAsync<string>(
            "localStorage.getItem",
            "authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}