namespace RouterPlus.Models;

public class SmsThread
{
    public string Number { get; set; } = string.Empty;
    public List<Message?> Messages { get; set; } = new();

    public Message? LastMessage => Messages.Last();

    public string PreviewMessage => Messages.LastOrDefault()?.Content.Length > 25
        ? Messages.LastOrDefault()?.Content.Substring(0, 25) + "..."
        : Messages.LastOrDefault()?.Content ?? "";
}