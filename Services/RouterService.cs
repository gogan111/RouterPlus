using System.Text.RegularExpressions;
using RouterPlus.ApiClients;
using RouterPlus.Converters;
using RouterPlus.Dtos.Requests;
using RouterPlus.Dtos.Responses;
using RouterPlus.Models;
using static System.Text.RegularExpressions.Regex;

namespace RouterPlus.Services;

public class RouterService
{
    private IRouterApi? _routerApi;

    public async Task<bool> AuthenticateAsync(string url, string password)
    {
        var normalizedUrl = NormalizeUrl(url);
        IRouterApi? routerApi = RouterApiFactory.Create(normalizedUrl);

        var request = new LoginRequest(password).ToDictionary();

        var response = await routerApi.LoginAsync(request);
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

    public async Task<List<Message>> getAllSmsMessages()
    {
        var request = new GetAllSmsMessagesRequest();

        var response = await _routerApi.GetAllSmsMessages(request);

        return MessageConverter.FromListDto(response.Content?.Messages);
    }

    private string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        return IsMatch(url, @"^https?://", RegexOptions.IgnoreCase)
            ? url
            : $"http://{url}";
    }
}