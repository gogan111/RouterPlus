using System.Globalization;
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
        if (response is { IsSuccessStatusCode: true, Content.Status: OperationResponse.ResponseStatus.SUCCESS })
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

    public async Task<bool> sendMessage(string newSmsMessage, string number)
    {
        var encodedMessage = SmsHexDecoder.EncodeSmsHex(newSmsMessage);
        var currentDate = ConvertDate(DateTime.Now);

        var sendMessageRequest = new SendMessageRequest(number, encodedMessage, currentDate);
        var response = await _routerApi.SendMessage(sendMessageRequest.ToDictionary());
        if (response is { IsSuccessStatusCode: true, Content.Status: OperationResponse.ResponseStatus.SUCCESS })
        {
            return true;
        }

        Console.WriteLine($"Authentication failed: {response.Content}");
        return false;
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
    
    private string ConvertDate(DateTime date)
    {
        string format = "yy;MM;dd;HH;mm;ss;z";
        return date.ToString(format, CultureInfo.InvariantCulture);
    }
}
