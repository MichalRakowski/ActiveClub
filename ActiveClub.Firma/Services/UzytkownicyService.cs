using ActiveClub.Shared.DTOs.Uzytkownicy;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class UzytkownicyService
    {
        private readonly HttpClient _httpClient;

        public UzytkownicyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UzytkownikDto>> GetAllAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<UzytkownikDto>>("api/uzytkownicy");
            return result ?? new List<UzytkownikDto>();
        }

        public async Task<UzytkownikDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UzytkownikDto>($"api/uzytkownicy/{id}");
        }

        public async Task<bool> CreateAsync(CreateUzytkownikDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/uzytkownicy", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się utworzyć użytkownika."
                    : error);
            }

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateUzytkownikDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/uzytkownicy/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się zaktualizować użytkownika."
                    : error);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/uzytkownicy/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Nie udało się usunąć użytkownika."
                    : error);
            }

            return true;
        }
    }
}