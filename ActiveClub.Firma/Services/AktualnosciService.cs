using ActiveClub.Shared.DTOs.Aktualnosci;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services;

public class AktualnosciService
{
    private readonly HttpClient _httpClient;

    public AktualnosciService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AktualnoscDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AktualnoscDto>>("api/Aktualnosci")
               ?? new List<AktualnoscDto>();
    }

    public async Task<bool> CreateAsync(CreateAktualnoscDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Aktualnosci", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(int id, UpdateAktualnoscDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Aktualnosci/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Aktualnosci/{id}");
        return response.IsSuccessStatusCode;
    }
}