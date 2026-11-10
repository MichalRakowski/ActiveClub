using ActiveClub.Shared.DTOs.Kluby;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class KlubyService
    {
        private readonly HttpClient _httpClient;

        public KlubyService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<KlubDto>> GetAllAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<KlubDto>>("api/kluby");
                return result ?? new List<KlubDto>();
            }
            catch
            {
                return new List<KlubDto>();
            }
        }

        public async Task<bool> CreateAsync(CreateKlubDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/kluby", dto);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(UpdateKlubDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/kluby", dto);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/kluby/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}