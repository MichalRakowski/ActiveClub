using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Uzytkownicy;

namespace ActiveClub.PortalWWW.Services.Api;

public class UzytkownicyApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UzytkownicyApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<UzytkownikDto>> GetUzytkownicyAsync()
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<List<UzytkownikDto>>("api/Uzytkownicy")
               ?? new List<UzytkownikDto>();
    }

    public async Task<bool> CreateAsync(CreateUzytkownikDto dto)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        var response = await client.PostAsJsonAsync("api/Uzytkownicy", dto);
        return response.IsSuccessStatusCode;
    }
    public async Task<UzytkownikDto?> GetByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<UzytkownikDto>(
            $"api/Uzytkownicy/{id}");
    }
    public async Task<bool> UpdateAsync(int id, UpdateUzytkownikDto dto)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        var response = await client.PutAsJsonAsync($"api/Uzytkownicy/{id}", dto);

        return response.IsSuccessStatusCode;
    }
}