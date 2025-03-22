using System.Text.Json.Serialization;

namespace RouterPlus.Dtos.Responses;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MessageDto
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("number")] public string Number { get; set; } = string.Empty;

    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;

    [JsonPropertyName("tag")] public SmsTags Tag { get; set; }

    [JsonPropertyName("date")] public string Date { get; set; } = string.Empty;

    [JsonPropertyName("draft_group_id")] public string? DraftGroupId { get; set; }

    [JsonPropertyName("received_all_concat_sms")]
    public int ReceivedAllConcatSms { get; set; }

    [JsonPropertyName("concat_sms_total")] public int ConcatSmsTotal { get; set; }

    [JsonPropertyName("concat_sms_received")]
    public int ConcatSmsReceived { get; set; }

    [JsonPropertyName("sms_class")] public int SmsClass { get; set; }
}