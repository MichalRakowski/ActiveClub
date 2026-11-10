using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Auth;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ActiveClub.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ActiveClubContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ActiveClubContext dbContext,
            JwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto)
        {
            ApplicationUser? user = null;

            try
            {
                var existingUser = await _userManager.FindByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    return (false, new[] { "Użytkownik o podanym adresie e-mail już istnieje." });
                }

                user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Imie = dto.Imie,
                    Nazwisko = dto.Nazwisko,
                    PhoneNumber = dto.Telefon,
                    CzyAktywny = true,
                    KlubDomyslnyId = null
                };

                var createResult = await _userManager.CreateAsync(user, dto.Haslo);
                if (!createResult.Succeeded)
                {
                    return (false, createResult.Errors.Select(e => e.Description));
                }

                if (!await _userManager.IsInRoleAsync(user, "Klient"))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Klient");

                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);
                        return (false, roleResult.Errors.Select(e => e.Description));
                    }
                }

                return (true, Enumerable.Empty<string>());
            }
            catch (Exception ex)
            {
                if (user != null)
                {
                    try
                    {
                        await _userManager.DeleteAsync(user);
                    }
                    catch (Exception deleteEx)
                    {
                        _logger.LogError(deleteEx, "Nie udało się usunąć użytkownika po błędzie rejestracji {Email}", dto.Email);
                    }
                }

                _logger.LogError(ex, "Błąd podczas rejestracji użytkownika {Email}", dto.Email);

                return (false, new[] { "Wystąpił błąd podczas rejestracji użytkownika." });
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Login)
                       ?? await _userManager.FindByNameAsync(dto.Login);

            if (user == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = "Nieprawidłowy login lub hasło."
                };
            }

            if (!user.CzyAktywny)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = "Konto jest nieaktywne."
                };
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Haslo);
            if (!validPassword)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    ErrorMessage = "Nieprawidłowy login lub hasło."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var rola = roles.FirstOrDefault();

            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDto
            {
                Success = true,
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Stanowisko = null,
                Rola = rola,
                Token = token
            };
        }
    }
}