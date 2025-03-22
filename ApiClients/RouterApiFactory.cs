using System.Net.Http;
using Refit;

namespace RouterPlus.ApiClients;

public static class RouterApiFactory
{
    public static IRouterApi? Create(string baseUrl)
    {
        var handler = new RefererHeaderHandler(baseUrl)
        {
            InnerHandler = new HttpClientHandler()
        };

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
        var settings = new RefitSettings(new NewtonsoftJsonContentSerializer());
        return RestService.For<IRouterApi>(httpClient, settings);
    }
}