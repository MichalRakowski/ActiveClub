using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using System.Net.Http;
using System.Net.Http.Json;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class RezerwacjeTreneraApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RezerwacjeTreneraApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(CreateRezerwacjaTreneraDto dto)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            var response = await client.PostAsJsonAsync(
                "api/RezerwacjeTrenera",
                dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<RezerwacjaTreneraDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            return await client.GetFromJsonAsync<List<RezerwacjaTreneraDto>>(
                $"api/RezerwacjeTrenera/uzytkownik/{uzytkownikId}")
                ?? new List<RezerwacjaTreneraDto>();
        }
        public async Task<RezerwacjaTreneraDto?> GetByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            return await client.GetFromJsonAsync<RezerwacjaTreneraDto>(
                $"api/RezerwacjeTrenera/{id}");
        }
    }
}