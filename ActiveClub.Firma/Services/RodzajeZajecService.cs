using ActiveClub.Shared.DTOs.RodzajeZajec;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class RodzajeZajecService
    {
        private readonly HttpClient _httpClient;

        public RodzajeZajecService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RodzajZajecDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RodzajZajecDto>>("api/rodzajezajec")
                   ?? new List<RodzajZajecDto>();
        }

        public async Task<bool> CreateAsync(CreateRodzajZajecDto dto)
        {
            var res = await _httpClient.PostAsJsonAsync("api/rodzajezajec", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRodzajZajecDto dto)
        {
            var res = await _httpClient.PutAsJsonAsync($"api/rodzajezajec/{id}", dto);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"api/rodzajezajec/{id}");
            return res.IsSuccessStatusCode;
        }
    }
}