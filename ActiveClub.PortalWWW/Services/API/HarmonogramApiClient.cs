using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Harmonogram;

namespace ActiveClub.PortalWWW.Services.Api;

public class HarmonogramApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HarmonogramApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<HarmonogramZajecListDto>> GetAsync(int? klubId, DateTime dzien)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        var url = $"api/HarmonogramZajec/publiczny?dzien={dzien:yyyy-MM-dd}";

        if (klubId.HasValue)
            url += $"&klubId={klubId.Value}";

        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Harmonogram błąd: {response.StatusCode} - {content}");
        }

        return await response.Content.ReadFromJsonAsync<List<HarmonogramZajecListDto>>()
               ?? new List<HarmonogramZajecListDto>();
    }
}