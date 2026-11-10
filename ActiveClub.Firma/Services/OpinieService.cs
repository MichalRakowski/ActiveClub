using ActiveClub.Shared.DTOs.Opinie;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class OpinieService
    {
        private readonly HttpClient _httpClient;

        public OpinieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OpiniaDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/opinie");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET OPINIE STATUS: {response.StatusCode}");
            Console.WriteLine($"GET OPINIE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<OpiniaDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<OpiniaDto>();
        }

        public async Task<OpiniaDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/opinie/{id}");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET OPINIA BY ID STATUS: {response.StatusCode}");
            Console.WriteLine($"GET OPINIA BY ID RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<OpiniaDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<bool> UpdateAsync(int id, UpdateOpiniaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/opinie/{id}", dto);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"PUT OPINIE STATUS: {response.StatusCode}");
            Console.WriteLine($"PUT OPINIE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/opinie/{id}");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"DELETE OPINIE STATUS: {response.StatusCode}");
            Console.WriteLine($"DELETE OPINIE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return true;
        }
    }
}