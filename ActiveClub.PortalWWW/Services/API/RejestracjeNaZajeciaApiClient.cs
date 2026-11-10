using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using System.Net.Http.Json;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class RejestracjeNaZajeciaApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RejestracjeNaZajeciaApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(CreateRejestracjaNaZajeciaDto dto)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            var response = await client.PostAsJsonAsync(
                "api/RejestracjeNaZajecia",
                dto);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateStatusAsync(int rejestracjaId, string status)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            var current = await client.GetFromJsonAsync<RejestracjaNaZajeciaDto>(
                $"api/RejestracjeNaZajecia/{rejestracjaId}");

            if (current == null)
                return false;

            var dto = new UpdateRejestracjaNaZajeciaDto
            {
                HarmonogramId = current.HarmonogramId,
                UzytkownikId = current.UzytkownikId,
                Status = status,
                DataRejestracji = current.DataRejestracji,
                Zrodlo = current.Zrodlo
            };

            var response = await client.PutAsJsonAsync(
                $"api/RejestracjeNaZajecia/{rejestracjaId}",
                dto);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<RejestracjaNaZajeciaDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            return await client.GetFromJsonAsync<List<RejestracjaNaZajeciaDto>>(
                $"api/RejestracjeNaZajecia/uzytkownik/{uzytkownikId}")
                ?? new List<RejestracjaNaZajeciaDto>();
        }
    }
}