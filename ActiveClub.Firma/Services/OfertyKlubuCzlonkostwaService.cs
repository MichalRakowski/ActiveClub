using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class OfertyKlubuCzlonkostwaService
    {
        private readonly HttpClient _httpClient;

        public OfertyKlubuCzlonkostwaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OfertaKlubuCzlonkostwoDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/ofertyklubuczlonkostwa");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<OfertaKlubuCzlonkostwoDto>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<OfertaKlubuCzlonkostwoDto>();
        }

        public async Task<OfertaKlubuCzlonkostwoDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<OfertaKlubuCzlonkostwoDto>($"api/ofertyklubuczlonkostwa/{id}");
        }

        public async Task<OfertaKlubuCzlonkostwoFormDataDto?> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<OfertaKlubuCzlonkostwoFormDataDto>("api/ofertyklubuczlonkostwa/form-data");
        }

        public async Task<bool> CreateAsync(CreateOfertaKlubuCzlonkostwoDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ofertyklubuczlonkostwa", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdateOfertaKlubuCzlonkostwoDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ofertyklubuczlonkostwa/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/ofertyklubuczlonkostwa/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}