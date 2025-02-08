using System.Net.Http;

namespace RouterPlus.ApiClients;

public class RefererHeaderHandler(string baseUrl) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("Referer", $"{baseUrl}/index.html");

        return await base.SendAsync(request, cancellationToken);
    }
}