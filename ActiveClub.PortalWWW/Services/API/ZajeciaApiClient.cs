using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Zajecia;

namespace ActiveClub.PortalWWW.Services.Api;

public class ZajeciaApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZajeciaApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<ZajecieDto>> GetZajeciaAsync()
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");
        return await client.GetFromJsonAsync<List<ZajecieDto>>("api/Zajecia")
               ?? new List<ZajecieDto>();
    }
}