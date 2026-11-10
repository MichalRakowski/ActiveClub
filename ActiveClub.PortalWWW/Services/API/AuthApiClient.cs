using System.Net.Http.Json;
using ActiveClub.Shared.DTOs.Auth;

namespace ActiveClub.PortalWWW.Services.Api
{
    public class AuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ActiveClubApi");
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/rejestracja", dto);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                var errorContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(errorContent))
                    return (false, "Wystąpił błąd podczas rejestracji.");

                return (false, errorContent);
            }
            catch (Exception ex)
            {
                return (false, $"Błąd połączenia z API: {ex.Message}");
            }
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = string.IsNullOrWhiteSpace(error)
                        ? "Nieprawidłowy login lub hasło."
                        : error
                };
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            return result ?? new LoginResponseDto
            {
                Success = false,
                ErrorMessage = "Błąd logowania."
            };
        }
    }
}