using System.Net.Http;
using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Trenerzy;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class TrenerzyApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TrenerzyApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<TrenerDto>> GetAsync()
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            try
            {
                var response = await client.GetAsync("api/trenerzy");

                if (!response.IsSuccessStatusCode)
                    return new List<TrenerDto>();

                var data = await response.Content.ReadFromJsonAsync<List<TrenerDto>>();
                return data ?? new List<TrenerDto>();
            }
            catch
            {
                return new List<TrenerDto>();
            }
        }

        public async Task<TrenerDto?> GetByIdAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ActiveClubApi");

            try
            {
                var response = await client.GetAsync($"api/trenerzy/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<TrenerDto>();
            }
            catch
            {
                return null;
            }
        }
    }
}