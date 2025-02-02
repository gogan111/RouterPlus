using System.Net.Http;
using System.Text;

namespace RouterPlus.Services;

public class RouterService
{
    private readonly HttpClient _httpClient;

    public RouterService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<bool> AuthenticateAsync(string url, string password)
    {
        try
        {
            string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("isTest", "false"),
                new KeyValuePair<string, string>("goformId", "LOGIN"),
                new KeyValuePair<string, string>("password", encodedPassword)
            });

            string endpoint = $"{url}/goform/goform_set_cmd_process";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint) { Content = formData };
            requestMessage.Headers.Add("Referer", $"{url}/index.html");

            var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode &&
                   (await response.Content.ReadAsStringAsync()).Contains("\"result\":\"0\"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication failed: {ex.Message}");
            return false;
        }
    }
}