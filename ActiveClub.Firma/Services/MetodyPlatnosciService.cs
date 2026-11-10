using ActiveClub.Shared.DTOs.MetodyPlatnosci;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class MetodyPlatnosciService
    {
        private readonly HttpClient _httpClient;

        public MetodyPlatnosciService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MetodaPlatnosciDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/metodyplatnosci");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET STATUS: {response.StatusCode}");
            Console.WriteLine($"GET RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<MetodaPlatnosciDto>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MetodaPlatnosciDto>();
        }

        public async Task<MetodaPlatnosciDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<MetodaPlatnosciDto>($"api/metodyplatnosci/{id}");
        }

        public async Task<(bool Success, string ResponseText)> CreateDebugAsync(CreateMetodaPlatnosciDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/metodyplatnosci", dto);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"CLIENT STATUS: {response.StatusCode}");
            Console.WriteLine($"CLIENT RESPONSE: {content}");

            return (response.IsSuccessStatusCode, content);
        }

        public async Task<bool> CreateAsync(CreateMetodaPlatnosciDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/metodyplatnosci", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateMetodaPlatnosciDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/metodyplatnosci/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/metodyplatnosci/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}