using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class RezerwacjeTreneraService
    {
        private readonly HttpClient _httpClient;

        public RezerwacjeTreneraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RezerwacjaTreneraDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RezerwacjaTreneraDto>>("api/rezerwacjetrenera")
                   ?? new List<RezerwacjaTreneraDto>();
        }

        public async Task<List<RezerwacjaTreneraDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            return await _httpClient.GetFromJsonAsync<List<RezerwacjaTreneraDto>>($"api/rezerwacjetrenera/uzytkownik/{uzytkownikId}")
                   ?? new List<RezerwacjaTreneraDto>();
        }

        public async Task<bool> CreateAsync(CreateRezerwacjaTreneraDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/rezerwacjetrenera", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRezerwacjaTreneraDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/rezerwacjetrenera/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/rezerwacjetrenera/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}