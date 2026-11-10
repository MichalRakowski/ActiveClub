using System.Net.Http;
using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Aktualnosci;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class AktualnosciApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AktualnosciApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<AktualnoscDto>> GetAktualnosciAsync()
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            try
            {
                var response = await client.GetAsync("api/Aktualnosci");

                if (!response.IsSuccessStatusCode)
                    return new List<AktualnoscDto>();

                var data = await response.Content.ReadFromJsonAsync<List<AktualnoscDto>>();

                return data ?? new List<AktualnoscDto>();
            }
            catch
            {
                return new List<AktualnoscDto>();
            }
        }
    }
}