using RouterPlus.Dtos;

namespace RouterPlus.Models;

public class Message
{
    public int Id { get; set; }

    public string Number { get; set; }

    public string Content { get; set; } = string.Empty;

    public SmsTags Tag { get; set; }

    public DateTime? Date { get; set; }

    public string? DraftGroupId { get; set; }

    public int ReceivedAllConcatSms { get; set; }

    public int ConcatSmsTotal { get; set; }

    public int ConcatSmsReceived { get; set; }

    public int SmsClass { get; set; }
}