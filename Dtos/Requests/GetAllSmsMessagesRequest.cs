using Refit;

namespace RouterPlus.Dtos.Requests;

public class GetAllSmsMessagesRequest
{
    [AliasAs("cmd")]
    public string Cmd { get; } = "sms_data_total";

    [AliasAs("page")]
    public int Page { get; set; } = 0;

    [AliasAs("data_per_page")]
    public int PageSize { get; set; } = 100;

    [AliasAs("mem_store")]
    public MemoryStore MemStore { get; set; } = MemoryStore.DeviceMemory;

    [AliasAs("tags")]
    public SmsTags Tags { get; set; } = SmsTags.All;

    [AliasAs("order_by")]
    public string OrderBy { get; set; } = "order+by+id+desc";
    
}