using System.Text;

namespace RouterPlus.Dtos.Requests;

public class LoginRequest(string password) : IFormUrlEncodedRequest
{
    private bool IsTest { get; set; } = false;

    private string GoformId { get; } = "LOGIN";

    private string Password { get; } = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { "isTest", IsTest.ToString() },
            { "goformId", GoformId },
            { "password", Password }
        };
    }
}