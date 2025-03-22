namespace RouterPlus.Dtos.Requests;

public interface IFormUrlEncodedRequest
{
    Dictionary<string, string> ToDictionary();
}