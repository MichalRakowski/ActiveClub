using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http;

namespace ActiveClub.Firma.Services
{
    public class RejestracjeNaZajeciaService
    {
        private readonly HttpClient _httpClient;

        public RejestracjeNaZajeciaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RejestracjaNaZajeciaDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/RejestracjeNaZajecia");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"GET REJESTRACJE STATUS: {response.StatusCode}");
            Console.WriteLine($"GET REJESTRACJE RESPONSE: {content}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Błąd API: {(int)response.StatusCode} - {content}");

            return JsonSerializer.Deserialize<List<RejestracjaNaZajeciaDto>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<RejestracjaNaZajeciaDto>();
        }

        public async Task<RejestracjaNaZajeciaDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<RejestracjaNaZajeciaDto>($"api/RejestracjeNaZajecia/{id}");
        }

        public async Task<RejestracjaNaZajeciaFormDataDto?> GetFormDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<RejestracjaNaZajeciaFormDataDto>("api/RejestracjeNaZajecia/form-data");
        }

        public async Task<bool> CreateAsync(CreateRejestracjaNaZajeciaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/RejestracjeNaZajecia", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się utworzyć rejestracji."
                    : error);
            }

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateRejestracjaNaZajeciaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/RejestracjeNaZajecia/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się zaktualizować rejestracji."
                    : error);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/RejestracjeNaZajecia/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}