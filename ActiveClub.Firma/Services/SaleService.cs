using ActiveClub.Shared.DTOs.Sale;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class SaleService
    {
        private readonly HttpClient _httpClient;

        public SaleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SalaDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/sale");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET SALE STATUS: {response.StatusCode}");
            Console.WriteLine($"GET SALE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<SalaDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SalaDto>();
        }

        public async Task<SalaDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SalaDto>($"api/sale/{id}");
        }

        public async Task<bool> CreateAsync(CreateSalaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/sale", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSalaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/sale/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/sale/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}