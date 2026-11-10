using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Auth;

namespace ActiveClub.PortalWWW.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/rejestracja", dto);

            if (response.IsSuccessStatusCode)
            {
                return (true, new List<string>());
            }

            var errors = new List<string>();

            try
            {
                var content = await response.Content.ReadFromJsonAsync<List<string>>();
                if (content != null && content.Any())
                {
                    errors.AddRange(content);
                    return (false, errors);
                }
            }
            catch
            {
            }

            errors.Add("Nie udało się zarejestrować użytkownika.");
            return (false, errors);
        }
    }
}