using Refit;

namespace RouterPlus.Dtos.Requests;

public class SendMessageRequest(string number, string messageBody, string sentAt)
{
    [AliasAs("goformId")] public string GoformId { get; } = "SEND_SMS";

    [AliasAs("encode_type")] public string EncodedType { get; } = "GSM7_default";

    [AliasAs("isTest")] public bool IsTest { get; } = false;

    [AliasAs("ID")] public int Id { get; } = -1;

    [AliasAs("Number")] public string Number { get; } = number;

    [AliasAs("MessageBody")] public string MessageBody { get; } = messageBody;

    [AliasAs("sms_time")] public string SentAt { get; } = sentAt;

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { "isTest", IsTest.ToString() },
            { "ID", Id.ToString() },
            { "encode_type", EncodedType },
            { "goformId", GoformId },
            { "Number", Number },
            { "MessageBody", MessageBody },
            { "sms_time", SentAt }
        };
    }
}