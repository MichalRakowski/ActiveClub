using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;

namespace ActiveClub.PortalWWW.Services.Api;

public class RodzajeCzlonkostwaApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RodzajeCzlonkostwaApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<RodzajCzlonkostwaDto>> GetAsync()
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        try
        {
            return await client.GetFromJsonAsync<List<RodzajCzlonkostwaDto>>("api/RodzajeCzlonkostwa")
                   ?? new List<RodzajCzlonkostwaDto>();
        }
        catch
        {
            return new List<RodzajCzlonkostwaDto>();
        }
    }
}