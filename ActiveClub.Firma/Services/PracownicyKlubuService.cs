using ActiveClub.Shared.DTOs.PracownicyKlubu;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class PracownicyKlubuService
    {
        private readonly HttpClient _httpClient;

        public PracownicyKlubuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PracownikKlubuDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/pracownicyklubu");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET PRACOWNICYKLUBU STATUS: {response.StatusCode}");
            Console.WriteLine($"GET PRACOWNICYKLUBU RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<PracownikKlubuDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PracownikKlubuDto>();
        }

        public async Task<PracownikKlubuDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PracownikKlubuDto>($"api/pracownicyklubu/{id}");
        }

        public async Task<PracownikKlubuFormDataDto?> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<PracownikKlubuFormDataDto>("api/pracownicyklubu/form-data");
        }

        public async Task<bool> CreateAsync(CreatePracownikKlubuDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pracownicyklubu", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePracownikKlubuDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/pracownicyklubu/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pracownicyklubu/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}