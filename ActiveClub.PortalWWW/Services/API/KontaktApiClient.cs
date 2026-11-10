using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Kontakt;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class KontaktApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public KontaktApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> SendAsync(KontaktFormDto dto)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            var response = await client.PostAsJsonAsync("api/Kontakt", dto);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("BŁĄD API KONTAKT:");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(content);
            }

            return response.IsSuccessStatusCode;
        }
    }
}