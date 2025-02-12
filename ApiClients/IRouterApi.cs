using Refit;
using RouterPlus.Dtos.Responses;

namespace RouterPlus.ApiClients;

public interface IRouterApi
{
    private const string GOFORM_GET_CMS_PROCESS = "/goform/goform_get_cmd_process";
    private const string GOFORM_SET_CMS_PROCESS = "/goform/goform_set_cmd_process";
    
    [Post(GOFORM_SET_CMS_PROCESS)]
    Task<ApiResponse<LoginResponse>> LoginAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> request);
    
}