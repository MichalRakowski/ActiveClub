using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Kluby;

namespace ActiveClub.PortalWWW.Services.Api;

public class KlubyApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public KlubyApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<KlubDto>> GetKlubyAsync(string? q = null, string? miasto = null)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        var url = "api/Kluby";
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(q))
            query.Add($"q={Uri.EscapeDataString(q)}");

        if (!string.IsNullOrWhiteSpace(miasto))
            query.Add($"miasto={Uri.EscapeDataString(miasto)}");

        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        return await client.GetFromJsonAsync<List<KlubDto>>(url)
               ?? new List<KlubDto>();
    }

    public async Task<List<string>> GetMiastaAsync()
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<List<string>>("api/Kluby/miasta")
               ?? new List<string>();
    }

    public async Task<KlubDto?> GetKlubAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<KlubDto>($"api/Kluby/{id}");
    }
}