using ActiveClub.Shared.DTOs.HarmonogramZajec;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class HarmonogramZajecService
    {
        private readonly HttpClient _httpClient;

        public HarmonogramZajecService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HarmonogramZajecDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/harmonogramzajec");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET HARMONOGRAM STATUS: {response.StatusCode}");
            Console.WriteLine($"GET HARMONOGRAM RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<HarmonogramZajecDto>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<HarmonogramZajecDto>();
        }

        public async Task<HarmonogramZajecDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<HarmonogramZajecDto>($"api/harmonogramzajec/{id}");
        }

        public async Task<bool> CreateAsync(CreateHarmonogramZajecDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/harmonogramzajec",
                dto);

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"CREATE STATUS: {response.StatusCode}");
            Console.WriteLine($"CREATE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateHarmonogramZajecDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/harmonogramzajec/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/harmonogramzajec/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}