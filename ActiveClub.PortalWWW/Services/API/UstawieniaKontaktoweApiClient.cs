using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class UstawieniaKontaktoweApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UstawieniaKontaktoweApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UstawieniaKontaktoweDto?> GetAsync()
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            return await client.GetFromJsonAsync<UstawieniaKontaktoweDto>(
                "api/UstawieniaKontaktowe");
        }
    }
}