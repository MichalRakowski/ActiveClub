using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>("api/dashboard/users-count");
        }
    }
}