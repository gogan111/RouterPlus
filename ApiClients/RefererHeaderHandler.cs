using System.Net.Http;

namespace RouterPlus.ApiClients;

public class RefererHeaderHandler(string baseUrl) : DelegatingHandler
{
    //TODO temporally added logging
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("Referer", $"{baseUrl}/index.html");
        Console.WriteLine("Request:");
        Console.WriteLine($"Method: {request.Method}");
        Console.WriteLine($"URL: {request.RequestUri}");
        Console.WriteLine($"Headers: {request.Headers}");


        var response = await base.SendAsync(request, cancellationToken);
        // Log Response
        Console.WriteLine("Response:");
        Console.WriteLine($"Status Code: {response.StatusCode}");

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            // Console.WriteLine($"Response Body: {responseContent}");
        }

        return response;
    }
}