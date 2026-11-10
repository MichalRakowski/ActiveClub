using ActiveClub.Shared.DTOs.GodzinyOtwarcia;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class GodzinyOtwarciaService
    {
        private readonly HttpClient _httpClient;

        public GodzinyOtwarciaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GodzinyOtwarciaDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GodzinyOtwarciaDto>>("api/godzinyotwarcia")
                   ?? new List<GodzinyOtwarciaDto>();
        }

        public async Task<GodzinyOtwarciaDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<GodzinyOtwarciaDto>($"api/godzinyotwarcia/{id}");
        }

        public async Task<List<GodzinyOtwarciaDto>> GetByKlubIdAsync(int klubId)
        {
            return await _httpClient.GetFromJsonAsync<List<GodzinyOtwarciaDto>>(
                $"api/godzinyotwarcia/klub/{klubId}")
                ?? new List<GodzinyOtwarciaDto>();
        }

        public async Task<bool> CreateAsync(CreateGodzinyOtwarciaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/godzinyotwarcia", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd API: {response.StatusCode}\n{error}");
            }

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateGodzinyOtwarciaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/godzinyotwarcia/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Błąd API: {response.StatusCode}\n{error}");
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/godzinyotwarcia/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}