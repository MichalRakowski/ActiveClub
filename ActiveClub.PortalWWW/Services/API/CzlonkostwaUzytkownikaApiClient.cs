using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;

namespace ActiveClub.PortalWWW.Services.Api;

public class CzlonkostwaUzytkownikaApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CzlonkostwaUzytkownikaApiClient(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> ZakupAsync(
        ZakupKarnetuDto dto)
    {
        var client =
            _httpClientFactory.CreateClient("ActiveClubApi");

        var response = await client.PostAsJsonAsync(
            "api/CzlonkostwaUzytkownika/zakup",
            dto);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AnulujAsync(int czlonkostwoId)
    {
        var client = _httpClientFactory.CreateClient("ActiveClubApi");

        var response = await client.PutAsync(
            $"api/CzlonkostwaUzytkownika/{czlonkostwoId}/anuluj",
            null);

        return response.IsSuccessStatusCode;
    }

    public async Task<List<CzlonkostwoUzytkownikaDto>>
        GetByUzytkownikAsync(int uzytkownikId)
    {
        var client =
            _httpClientFactory.CreateClient("ActiveClubApi");

        return await client.GetFromJsonAsync<
            List<CzlonkostwoUzytkownikaDto>>(
            $"api/CzlonkostwaUzytkownika/uzytkownik/{uzytkownikId}")
            ?? new List<CzlonkostwoUzytkownikaDto>();
    }
}