using ActiveClub.Shared.DTOs.Trenerzy;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class TrenerzyService
    {
        private readonly HttpClient _httpClient;

        public TrenerzyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TrenerDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TrenerDto>>("api/trenerzy")
                   ?? new List<TrenerDto>();
        }

        public async Task<TrenerDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TrenerDto>($"api/trenerzy/{id}");
        }

        public async Task<bool> CreateAsync(CreateTrenerDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/trenerzy", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateTrenerDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/trenerzy/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/trenerzy/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}