using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.Firma.Services
{
    public class UstawieniaKontaktoweService
    {
        private readonly HttpClient _httpClient;

        public UstawieniaKontaktoweService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UstawieniaKontaktoweDto?> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<UstawieniaKontaktoweDto>(
                "api/UstawieniaKontaktowe");
        }

        public async Task<bool> UpdateAsync(UpdateUstawieniaKontaktoweDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync(
                "api/UstawieniaKontaktowe",
                dto);

            return response.IsSuccessStatusCode;
        }
    }
}