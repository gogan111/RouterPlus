using System.Runtime.Serialization;

namespace RouterPlus.Dtos;

public enum SmsTags
{
    [EnumMember(Value = "0")] Unread = 0,
    [EnumMember(Value = "1")] Received = 1,
    [EnumMember(Value = "2")] Sent = 2,
    [EnumMember(Value = "3")] Failed = 3,
    [EnumMember(Value = "4")] Draft = 4,
    [EnumMember(Value = "10")] All = 10
}