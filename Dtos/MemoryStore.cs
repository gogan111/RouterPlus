using System.Runtime.Serialization;

namespace RouterPlus.Dtos;

public enum MemoryStore
{
    [EnumMember(Value = "0")]
    SimMemory = 0,
    [EnumMember(Value = "1")]
    DeviceMemory = 1
}