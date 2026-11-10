using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Udogodnienia;

namespace ActiveClub.PortalWWW.Services.Api;

public class UdogodnieniaApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UdogodnieniaApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<UdogodnienieDto>> GetAsync()
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<List<UdogodnienieDto>>("api/Udogodnienia")
               ?? new List<UdogodnienieDto>();
    }
}