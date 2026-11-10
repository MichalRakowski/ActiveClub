using System.Net.Http;
using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Aktualnosci;

namespace ActiveClub.PortalWWW.Services
{
    public class AktualnosciService
    {
        private readonly HttpClient _http;

        public AktualnosciService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<AktualnoscDto>> PobierzAktualnosci()
        {
            return await _http.GetFromJsonAsync<List<AktualnoscDto>>(
                "http://localhost:5006/api/aktualnosci")
                ?? new List<AktualnoscDto>();
        }
    }
}