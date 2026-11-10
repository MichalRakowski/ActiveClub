using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace ActiveClub.Firma.Services
{
    public class CzlonkostwaUzytkownikaService
    {
        private readonly HttpClient _httpClient;

        public CzlonkostwaUzytkownikaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CzlonkostwoUzytkownikaDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/czlonkostwauzytkownika");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<CzlonkostwoUzytkownikaDto>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<CzlonkostwoUzytkownikaDto>();
        }
        public async Task<int> AnulujPrzeterminowaneAsync()
        {
            var response = await _httpClient.PostAsync(
                "api/CzlonkostwaUzytkownika/anuluj-przeterminowane",
                null);

            if (!response.IsSuccessStatusCode)
                return 0;

            return await response.Content.ReadFromJsonAsync<int>();
        }
        public async Task<CzlonkostwoUzytkownikaDto?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/czlonkostwauzytkownika/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.IsNullOrWhiteSpace(content)
                    ? "Nie udało się pobrać członkostwa."
                    : content);

            return JsonSerializer.Deserialize<CzlonkostwoUzytkownikaDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<CzlonkostwoUzytkownikaFormDataDto?> GetFormDataAsync()
        {
            var response = await _httpClient.GetAsync("api/czlonkostwauzytkownika/form-data");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.IsNullOrWhiteSpace(content)
                    ? "Nie udało się pobrać danych formularza."
                    : content);

            return JsonSerializer.Deserialize<CzlonkostwoUzytkownikaFormDataDto>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<LookupItemDto>> GetPracownicyKlubuAsync(int klubId)
        {
            var response = await _httpClient.GetAsync($"api/czlonkostwauzytkownika/pracownicy-klubu/{klubId}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(string.IsNullOrWhiteSpace(content)
                    ? "Nie udało się pobrać pracowników klubu."
                    : content);

            return JsonSerializer.Deserialize<List<LookupItemDto>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<LookupItemDto>();
        }

        public async Task<bool> CreateAsync(CreateCzlonkostwoUzytkownikaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/czlonkostwauzytkownika", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się utworzyć członkostwa."
                    : error);
            }

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateCzlonkostwoUzytkownikaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/czlonkostwauzytkownika/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się zaktualizować członkostwa."
                    : error);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/czlonkostwauzytkownika/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się usunąć członkostwa."
                    : error);
            }

            return true;
        }
        public async Task<bool> DodajWejscieAsync(int id)
        {
            var response = await _httpClient.PostAsync(
                $"api/CzlonkostwaUzytkownika/{id}/wejscie",
                null);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CofnijWejscieAsync(int id)
        {
            var response = await _httpClient.PostAsync(
                $"api/CzlonkostwaUzytkownika/{id}/cofnij-wejscie",
                null);

            return response.IsSuccessStatusCode;
        }
    }
}