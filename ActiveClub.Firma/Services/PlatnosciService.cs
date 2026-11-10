using ActiveClub.Shared.DTOs.Platnosci;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class PlatnosciService
    {
        private readonly HttpClient _httpClient;

        public PlatnosciService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PlatnoscDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/platnosci");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET PLATNOSCI STATUS: {response.StatusCode}");
            Console.WriteLine($"GET PLATNOSCI RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<PlatnoscDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PlatnoscDto>();
        }

        public async Task<PlatnoscDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PlatnoscDto>($"api/platnosci/{id}");
        }

        public async Task<PlatnoscFormDataDto> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<PlatnoscFormDataDto>("api/platnosci/form-data")
                   ?? new PlatnoscFormDataDto();
        }

        public async Task<bool> CreateAsync(CreatePlatnoscDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/platnosci", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePlatnoscDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/platnosci/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/platnosci/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}