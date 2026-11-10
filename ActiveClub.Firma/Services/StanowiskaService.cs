using ActiveClub.Shared.DTOs.Stanowiska;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class StanowiskaService
    {
        private readonly HttpClient _httpClient;

        public StanowiskaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StanowiskoDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/stanowiska");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET STANOWISKA STATUS: {response.StatusCode}");
            Console.WriteLine($"GET STANOWISKA RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<StanowiskoDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<StanowiskoDto>();
        }

        public async Task<StanowiskoDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<StanowiskoDto>($"api/stanowiska/{id}");
        }

        public async Task<bool> CreateAsync(CreateStanowiskoDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/stanowiska", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateStanowiskoDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/stanowiska/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/stanowiska/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}