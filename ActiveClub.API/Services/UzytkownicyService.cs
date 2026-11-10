using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Uzytkownicy;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class UzytkownicyService : IUzytkownicyService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ActiveClubContext _dbContext;

        public UzytkownicyService(
            UserManager<ApplicationUser> userManager,
            ActiveClubContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<List<UzytkownikDto>> GetAllAsync()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.Nazwisko)
                .ThenBy(u => u.Imie)
                .ToListAsync();

            var pracownicy = await _dbContext.PracownicyKlubus
                .AsNoTracking()
                .ToListAsync();

            var result = new List<UzytkownikDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var pracownik = pracownicy.FirstOrDefault(p => p.UzytkownikId == user.Id);

                result.Add(new UzytkownikDto
                {
                    Id = user.Id,
                    Imie = user.Imie,
                    Nazwisko = user.Nazwisko,
                    Email = user.Email ?? string.Empty,
                    Telefon = user.PhoneNumber,
                    CzyAktywny = user.CzyAktywny,
                    KlubDomyslnyId = user.KlubDomyslnyId,
                    Uwagi = user.Uwagi,
                    Rola = roles.FirstOrDefault(),
                    KlubId = pracownik?.KlubId,
                    StanowiskoId = pracownik?.StanowiskoId
                });
            }

            return result;
        }

        public async Task<UzytkownikDto?> GetByIdAsync(int id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var pracownik = await _dbContext.PracownicyKlubus
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UzytkownikId == id);

            return new UzytkownikDto
            {
                Id = user.Id,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Email = user.Email ?? string.Empty,
                Telefon = user.PhoneNumber,
                CzyAktywny = user.CzyAktywny,
                KlubDomyslnyId = user.KlubDomyslnyId,
                Uwagi = user.Uwagi,
                Rola = roles.FirstOrDefault(),
                KlubId = pracownik?.KlubId,
                StanowiskoId = pracownik?.StanowiskoId
            };
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors, UzytkownikDto? User)> CreateAsync(CreateUzytkownikDto dto)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            ApplicationUser? user = null;

            try
            {
                var existing = await _userManager.FindByEmailAsync(dto.Email);
                if (existing != null)
                {
                    return (false, new[] { "Użytkownik z takim adresem e-mail już istnieje." }, null);
                }

                var allowedRoles = new[] { "Klient", "Pracownik", "Trener", "Admin" };

                if (string.IsNullOrWhiteSpace(dto.Rola) || !allowedRoles.Contains(dto.Rola))
                {
                    return (false, new[] { "Wybrano nieprawidłową rolę." }, null);
                }

                var requiresEmployeeData =
                    dto.Rola == "Pracownik" ||
                    dto.Rola == "Trener" ||
                    dto.Rola == "Admin";

                if (requiresEmployeeData)
                {
                    if (!dto.KlubId.HasValue)
                    {
                        return (false, new[] { "Dla tej roli musisz wybrać klub." }, null);
                    }

                    if (!dto.StanowiskoId.HasValue)
                    {
                        return (false, new[] { "Dla tej roli musisz wybrać stanowisko." }, null);
                    }

                    var klub = await _dbContext.Klubies
                        .AsNoTracking()
                        .FirstOrDefaultAsync(k => k.KlubId == dto.KlubId.Value);

                    if (klub == null)
                    {
                        return (false, new[] { "Wybrany klub nie istnieje." }, null);
                    }

                    var stanowisko = await _dbContext.Stanowiskas
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StanowiskoId == dto.StanowiskoId.Value && s.CzyAktywne);

                    if (stanowisko == null)
                    {
                        return (false, new[] { "Wybrane stanowisko nie istnieje lub jest nieaktywne." }, null);
                    }
                }

                user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    Imie = dto.Imie,
                    Nazwisko = dto.Nazwisko,
                    PhoneNumber = dto.Telefon,
                    CzyAktywny = dto.CzyAktywny,
                    KlubDomyslnyId = dto.KlubDomyslnyId,
                    Uwagi = dto.Uwagi
                };

                var result = await _userManager.CreateAsync(user, dto.Haslo);

                if (!result.Succeeded)
                {
                    return (false, result.Errors.Select(e => e.Description), null);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Rola);

                if (!addRoleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    await transaction.RollbackAsync();
                    return (false, addRoleResult.Errors.Select(e => e.Description), null);
                }

                if (requiresEmployeeData)
                {
                    var pracownik = new PracownicyKlubu
                    {
                        UzytkownikId = user.Id,
                        KlubId = dto.KlubId!.Value,
                        StanowiskoId = dto.StanowiskoId!.Value,
                        CzyAktywny = dto.CzyAktywny,
                        Uwagi = dto.Uwagi
                    };

                    _dbContext.PracownicyKlubus.Add(pracownik);

                    if (dto.Rola == "Trener")
                    {
                        var trener = new Trenerzy
                        {
                            UzytkownikId = user.Id,
                            Imie = dto.Imie,
                            Nazwisko = dto.Nazwisko,
                            StawkaZaGodzine = 0,
                            CzyAktywny = dto.CzyAktywny,
                            DataUtworzenia = DateTime.UtcNow
                        };

                        _dbContext.Trenerzies.Add(trener);
                    }

                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                var mapped = new UzytkownikDto
                {
                    Id = user.Id,
                    Imie = user.Imie,
                    Nazwisko = user.Nazwisko,
                    Email = user.Email ?? string.Empty,
                    Telefon = user.PhoneNumber,
                    CzyAktywny = user.CzyAktywny,
                    KlubDomyslnyId = user.KlubDomyslnyId,
                    Uwagi = user.Uwagi,
                    Rola = dto.Rola,
                    KlubId = dto.KlubId,
                    StanowiskoId = dto.StanowiskoId
                };

                return (true, Array.Empty<string>(), mapped);
            }
            catch
            {
                await transaction.RollbackAsync();

                if (user != null)
                {
                    try
                    {
                        await _userManager.DeleteAsync(user);
                    }
                    catch
                    {
                    }
                }

                return (false, new[] { "Wystąpił błąd podczas tworzenia użytkownika." }, null);
            }
        }

        public async Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(int id, UpdateUzytkownikDto dto)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return (false, new[] { "Nie znaleziono użytkownika." });

                var otherWithEmail = await _userManager.FindByEmailAsync(dto.Email);
                if (otherWithEmail != null && otherWithEmail.Id != id)
                    return (false, new[] { "Inny użytkownik ma już ten adres e-mail." });

                var allowedRoles = new[] { "Klient", "Pracownik", "Trener", "Admin" };

                if (string.IsNullOrWhiteSpace(dto.Rola) || !allowedRoles.Contains(dto.Rola))
                    return (false, new[] { "Wybrano nieprawidłową rolę." });

                var requiresEmployeeData =
                    dto.Rola == "Pracownik" ||
                    dto.Rola == "Trener" ||
                    dto.Rola == "Admin";

                if (requiresEmployeeData)
                {
                    if (!dto.KlubId.HasValue)
                        return (false, new[] { "Dla tej roli musisz wybrać klub." });

                    if (!dto.StanowiskoId.HasValue)
                        return (false, new[] { "Dla tej roli musisz wybrać stanowisko." });

                    var klub = await _dbContext.Klubies
                        .AsNoTracking()
                        .FirstOrDefaultAsync(k => k.KlubId == dto.KlubId.Value);

                    if (klub == null)
                        return (false, new[] { "Wybrany klub nie istnieje." });

                    var stanowisko = await _dbContext.Stanowiskas
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StanowiskoId == dto.StanowiskoId.Value && s.CzyAktywne);

                    if (stanowisko == null)
                        return (false, new[] { "Wybrane stanowisko nie istnieje lub jest nieaktywne." });
                }

                user.Imie = dto.Imie;
                user.Nazwisko = dto.Nazwisko;
                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.PhoneNumber = dto.Telefon;
                user.CzyAktywny = dto.CzyAktywny;
                user.KlubDomyslnyId = dto.KlubDomyslnyId;
                user.Uwagi = dto.Uwagi;

                var updateUserResult = await _userManager.UpdateAsync(user);
                if (!updateUserResult.Succeeded)
                    return (false, updateUserResult.Errors.Select(e => e.Description));

                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (false, removeRolesResult.Errors.Select(e => e.Description));
                    }
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, dto.Rola);
                if (!addRoleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (false, addRoleResult.Errors.Select(e => e.Description));
                }

                var pracownik = await _dbContext.PracownicyKlubus
                    .FirstOrDefaultAsync(p => p.UzytkownikId == id);

                var trener = await _dbContext.Trenerzies
                    .FirstOrDefaultAsync(t => t.UzytkownikId == id);

                if (requiresEmployeeData)
                {
                    if (pracownik == null)
                    {
                        pracownik = new PracownicyKlubu
                        {
                            UzytkownikId = user.Id,
                            KlubId = dto.KlubId!.Value,
                            StanowiskoId = dto.StanowiskoId!.Value,
                            CzyAktywny = dto.CzyAktywny,
                            Uwagi = dto.Uwagi
                        };

                        _dbContext.PracownicyKlubus.Add(pracownik);
                    }
                    else
                    {
                        pracownik.KlubId = dto.KlubId!.Value;
                        pracownik.StanowiskoId = dto.StanowiskoId!.Value;
                        pracownik.CzyAktywny = dto.CzyAktywny;
                        pracownik.Uwagi = dto.Uwagi;
                    }

                    if (dto.Rola == "Trener")
                    {
                        if (trener == null)
                        {
                            trener = new Trenerzy
                            {
                                UzytkownikId = user.Id,
                                Imie = dto.Imie,
                                Nazwisko = dto.Nazwisko,
                                StawkaZaGodzine = 0,
                                CzyAktywny = dto.CzyAktywny,
                                DataUtworzenia = DateTime.UtcNow
                            };

                            _dbContext.Trenerzies.Add(trener);
                        }
                        else
                        {
                            trener.Imie = dto.Imie;
                            trener.Nazwisko = dto.Nazwisko;
                            trener.CzyAktywny = dto.CzyAktywny;
                        }
                    }
                    else
                    {
                        if (trener != null)
                            _dbContext.Trenerzies.Remove(trener);
                    }
                }
                else
                {
                    if (pracownik != null)
                        _dbContext.PracownicyKlubus.Remove(pracownik);

                    if (trener != null)
                        _dbContext.Trenerzies.Remove(trener);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, Array.Empty<string>());
            }
            catch
            {
                await transaction.RollbackAsync();
                return (false, new[] { "Wystąpił błąd podczas aktualizacji użytkownika." });
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return false;

                var pracownik = await _dbContext.PracownicyKlubus
                    .FirstOrDefaultAsync(p => p.UzytkownikId == id);

                if (pracownik != null)
                {
                    _dbContext.PracownicyKlubus.Remove(pracownik);
                }

                var trener = await _dbContext.Trenerzies
                    .FirstOrDefaultAsync(t => t.UzytkownikId == id);

                if (trener != null)
                {
                    _dbContext.Trenerzies.Remove(trener);
                }

                await _dbContext.SaveChangesAsync();

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}