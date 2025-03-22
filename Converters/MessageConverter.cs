using System.Globalization;
using RouterPlus.Dtos.Responses;
using RouterPlus.Models;

namespace RouterPlus.Converters;

public class MessageConverter
{
    public static Message? FromDto(MessageDto? dto)
    {
        if (dto == null)
        {
            return null;
        }

        var message = new Message();
        message.Number = SmsHexDecoder.DecodeSmsHex(dto.Number);
        message.Content = SmsHexDecoder.DecodeSmsHex(dto.Content);
        message.Id = dto.Id;
        message.Tag = dto.Tag;
        message.Date = ParseDate(dto.Date);

        return message;
    }

    public static List<Message> FromListDto(List<MessageDto>? list)
    {
        return list?.Select(FromDto)
                   .OfType<Message>()
                   .ToList()
               ?? new List<Message>();
    }
    
    private static DateTime? ParseDate(string dateString)
    {
        string format = "yy,MM,dd,HH,mm,ss,z";

        if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedResult))
        {
            return parsedResult;
        }

        return null;
    }
}