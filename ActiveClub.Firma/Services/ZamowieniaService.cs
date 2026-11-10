using ActiveClub.Shared.DTOs.Zamowienia;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class ZamowieniaService
    {
        private readonly HttpClient _httpClient;

        public ZamowieniaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ZamowienieDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/zamowienia");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET ZAMOWIENIA STATUS: {response.StatusCode}");
            Console.WriteLine($"GET ZAMOWIENIA RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<ZamowienieDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<ZamowienieDto>();
        }

        public async Task<ZamowienieDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ZamowienieDto>($"api/zamowienia/{id}");
        }

        public async Task<ZamowienieFormDataDto?> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<ZamowienieFormDataDto>("api/zamowienia/form-data");
        }

        public async Task<bool> CreateAsync(CreateZamowienieDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/zamowienia", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateZamowienieDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/zamowienia/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/zamowienia/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}