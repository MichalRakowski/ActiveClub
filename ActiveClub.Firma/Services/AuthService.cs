using ActiveClub.Shared.DTOs.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UserSession _session;

        public AuthService(HttpClient httpClient, UserSession session)
        {
            _httpClient = httpClient;
            _session = session;
        }

        public async Task<LoginResponseDto> LoginAsync(string login, string haslo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginDto
                {
                    Login = login,
                    Haslo = haslo
                });

                if (!response.IsSuccessStatusCode)
                {
                    _session.Clear();

                    try
                    {
                        var errorResult = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                        return errorResult ?? new LoginResponseDto
                        {
                            Success = false,
                            ErrorMessage = "Błąd logowania."
                        };
                    }
                    catch
                    {
                        return new LoginResponseDto
                        {
                            Success = false,
                            ErrorMessage = "Błąd logowania."
                        };
                    }
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (result is null)
                {
                    _session.Clear();

                    return new LoginResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Serwer zwrócił pustą odpowiedź."
                    };
                }

                if (result.Success && !string.IsNullOrWhiteSpace(result.Token))
                {
                    _session.SetToken(result.Token);
                }
                else
                {
                    _session.Clear();
                }

                return result;
            }
            catch (HttpRequestException)
            {
                _session.Clear();

                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = "Nie można połączyć się z serwerem."
                };
            }
            catch (Exception)
            {
                _session.Clear();

                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = "Wystąpił nieoczekiwany błąd."
                };
            }
        }

        public void Logout()
        {
            _session.Clear();
        }
    }
}