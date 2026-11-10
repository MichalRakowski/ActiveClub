using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class RodzajeCzlonkostwaService
    {
        private readonly HttpClient _httpClient;

        public RodzajeCzlonkostwaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RodzajCzlonkostwaDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RodzajCzlonkostwaDto>>("api/RodzajeCzlonkostwa")
                   ?? new List<RodzajCzlonkostwaDto>();
        }

        public async Task<(bool Success, string ErrorMessage)> CreateAsync(CreateRodzajCzlonkostwaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/RodzajeCzlonkostwa", dto);

            if (response.IsSuccessStatusCode)
                return (true, "");

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(int id, UpdateRodzajCzlonkostwaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/RodzajeCzlonkostwa/{id}", dto);

            if (response.IsSuccessStatusCode)
                return (true, "");

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/RodzajeCzlonkostwa/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"DeleteAsync ERROR: {(int)response.StatusCode} {response.StatusCode}");
                Debug.WriteLine(error);
            }

            return response.IsSuccessStatusCode;
        }
    }
}