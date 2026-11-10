using ActiveClub.Shared.DTOs.Rabaty;
using ActiveClub.Shared.JsonConverters;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class RabatyService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public RabatyService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _jsonOptions.Converters.Add(new DateOnlyJsonConverter());
            _jsonOptions.Converters.Add(new NullableDateOnlyJsonConverter());
        }

        public async Task<List<RabatDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/rabaty");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET RABATY STATUS: {response.StatusCode}");
            Console.WriteLine($"GET RABATY RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<RabatDto>>(content, _jsonOptions) ?? new List<RabatDto>();
        }

        public async Task<RabatDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/rabaty/{id}");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET BY ID RABATY STATUS: {response.StatusCode}");
            Console.WriteLine($"GET BY ID RABATY RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<RabatDto>(content, _jsonOptions);
        }

        public async Task<bool> CreateAsync(CreateRabatDto dto)
        {
            var json = JsonSerializer.Serialize(dto, _jsonOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"POST RABATY JSON: {json}");

            var response = await _httpClient.PostAsync("api/rabaty", httpContent);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"POST RABATY STATUS: {response.StatusCode}");
            Console.WriteLine($"POST RABATY RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRabatDto dto)
        {
            var json = JsonSerializer.Serialize(dto, _jsonOptions);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"PUT RABATY JSON: {json}");

            var response = await _httpClient.PutAsync($"api/rabaty/{id}", httpContent);
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"PUT RABATY STATUS: {response.StatusCode}");
            Console.WriteLine($"PUT RABATY RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/rabaty/{id}");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"DELETE RABATY STATUS: {response.StatusCode}");
            Console.WriteLine($"DELETE RABATY RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return true;
        }
    }
}