using ActiveClub.Shared.DTOs.Raporty;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class RaportyService
    {
        private readonly HttpClient _httpClient;

        public RaportyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RaportFormDataDto?> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<RaportFormDataDto>("api/raporty/form-data");
        }

        public async Task<List<RaportDefinicjaDto>> GetDefinitionsAsync()
        {
            var response = await _httpClient.GetAsync("api/raporty/definitions");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<RaportDefinicjaDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<RaportDefinicjaDto>();
        }

        public async Task<List<RaportWygenerowanyDto>> GetGeneratedAsync()
        {
            var response = await _httpClient.GetAsync("api/raporty/generated");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<RaportWygenerowanyDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<RaportWygenerowanyDto>();
        }

        public async Task<GenerateRaportResultDto?> GenerateAsync(GenerateRaportDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/raporty/generate", dto);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<GenerateRaportResultDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}