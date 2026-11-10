using ActiveClub.Shared.DTOs.PozycjeZamowienia;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class PozycjeZamowieniaService
    {
        private readonly HttpClient _httpClient;

        public PozycjeZamowieniaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PozycjaZamowieniaDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/PozycjeZamowienia");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET POZYCJE ZAMOWIENIA STATUS: {response.StatusCode}");
            Console.WriteLine($"GET POZYCJE ZAMOWIENIA RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<PozycjaZamowieniaDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PozycjaZamowieniaDto>();
        }

        public async Task<PozycjaZamowieniaDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PozycjaZamowieniaDto>($"api/PozycjeZamowienia/{id}");
        }

        public async Task<PozycjaZamowieniaFormDataDto> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<PozycjaZamowieniaFormDataDto>("api/PozycjeZamowienia/form-data")
                ?? new PozycjaZamowieniaFormDataDto();
        }

        public async Task<bool> CreateAsync(CreatePozycjaZamowieniaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/PozycjeZamowienia", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePozycjaZamowieniaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/PozycjeZamowienia/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/PozycjeZamowienia/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}