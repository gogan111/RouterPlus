using System.Text.Json.Serialization;
using Refit;

namespace RouterPlus.Dtos.Responses;

public class AllSmsMessagesResponse
{
    [JsonPropertyName("messages")]
    public List<MessageDto> Messages { get; set; }
}
