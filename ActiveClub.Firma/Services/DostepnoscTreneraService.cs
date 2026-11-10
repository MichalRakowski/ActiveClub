using ActiveClub.Shared.DTOs.DostepnoscTrenera;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class DostepnoscTreneraService
    {
        private readonly HttpClient _httpClient;

        public DostepnoscTreneraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DostepnoscTreneraDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DostepnoscTreneraDto>>("api/DostepnoscTrenera")
                   ?? new List<DostepnoscTreneraDto>();
        }

        public async Task<List<DostepnoscTreneraDto>> GetByTrenerIdAsync(int trenerId)
        {
            return await _httpClient.GetFromJsonAsync<List<DostepnoscTreneraDto>>($"api/DostepnoscTrenera/trener/{trenerId}")
                   ?? new List<DostepnoscTreneraDto>();
        }

        public async Task<bool> CreateAsync(CreateDostepnoscTreneraDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/DostepnoscTrenera", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDostepnoscTreneraDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/DostepnoscTrenera/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/DostepnoscTrenera/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}