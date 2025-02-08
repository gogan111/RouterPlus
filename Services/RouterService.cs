using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using RouterPlus.ApiClients;
using RouterPlus.Dtos.Responses;

namespace RouterPlus.Services;

public class RouterService
{
    private IRouterApi? _routerApi;

    public async Task<bool> AuthenticateAsync(string url, string password)
    {
        var normalizedUrl = NormalizeUrl(url);
        IRouterApi? routerApi = RouterApiFactory.Create(normalizedUrl);

        var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        var requestData = new Dictionary<string, string>
        {
            { "isTest", "false" },
            { "goformId", "LOGIN" },
            { "password", encodedPassword }
        };

        var response = await routerApi.LoginAsync(requestData);
        if (response is { IsSuccessStatusCode: true, Content.Status: LoginResponse.ResponseStatus.SUCCESS })
        {
            _routerApi = routerApi;
            return true;
        }
        Console.WriteLine($"Authentication failed: {response.Content}");
        return false;
    }

    public void Logout()
    {
        _routerApi = null;
    }

    public async Task<bool> getAllSmsMessages(int page,int pageSize)
    {
            return false;
    }

    private HttpRequestMessage GetRequestMessage(UriBuilder builder, FormUrlEncodedContent formData)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, builder.Uri) { Content = formData };
        requestMessage.Headers.Add("Referer", GetRefererUrl(builder));
        return requestMessage;
    }

    private string GetRefererUrl(UriBuilder builder)
    {
        Uri uri = builder.Uri;

        return $"{uri.Scheme}://{uri.Host}{(uri.IsDefaultPort ? string.Empty : $":{uri.Port}")}/index.html";
    }

    private string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        if (Regex.IsMatch(url, @"^https?://", RegexOptions.IgnoreCase))
            return url;

        return $"http://{url}";
    }
}