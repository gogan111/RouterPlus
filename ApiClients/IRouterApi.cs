using Refit;
using RouterPlus.Dtos.Requests;
using RouterPlus.Dtos.Responses;

namespace RouterPlus.ApiClients;

public interface IRouterApi
{
    private const string GOFORM_GET_CMS_PROCESS = "/goform/goform_get_cmd_process";
    private const string GOFORM_SET_CMS_PROCESS = "/goform/goform_set_cmd_process";

    [Post(GOFORM_SET_CMS_PROCESS)]
    Task<ApiResponse<OperationResponse>> LoginAsync(
        [Body(BodySerializationMethod.UrlEncoded)]
        Dictionary<string, string> request);

    [Get(GOFORM_GET_CMS_PROCESS)]
    Task<ApiResponse<AllSmsMessagesResponse>> GetAllSmsMessages([Query] GetAllSmsMessagesRequest request);

    [Post(GOFORM_SET_CMS_PROCESS)]
    Task<ApiResponse<OperationResponse>> SendMessage(
        [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
}