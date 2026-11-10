using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class RejestracjeNaZajeciaService : IRejestracjeNaZajeciaService
    {
        private readonly ActiveClubContext _context;

        public RejestracjeNaZajeciaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<RejestracjaNaZajeciaDto>> GetAllAsync()
        {
            try
            {
                return await _context.RejestracjeNaZajecia
                    .AsNoTracking()
                    .Include(x => x.Harmonogram)
                        .ThenInclude(h => h.RodzajZajec)
                    .Include(x => x.Harmonogram)
                        .ThenInclude(h => h.Sala)
                    .Include(x => x.Uzytkownik)
                    .OrderByDescending(x => x.DataRejestracji)
                    .Select(x => new RejestracjaNaZajeciaDto
                    {
                        RejestracjaId = x.RejestracjaId,
                        HarmonogramId = x.HarmonogramId,
                        UzytkownikId = x.UzytkownikId,
                        Status = x.Status ?? string.Empty,
                        DataRejestracji = x.DataRejestracji,
                        Zrodlo = x.Zrodlo ?? string.Empty,

                        HarmonogramNazwa =
                            (x.Harmonogram.RodzajZajec != null ? x.Harmonogram.RodzajZajec.Nazwa : "Zajęcia")
                            + " | "
                            + x.Harmonogram.StartAt.ToString("dd.MM.yyyy HH:mm")
                            + " - "
                            + x.Harmonogram.EndAt.ToString("HH:mm")
                            + (x.Harmonogram.Sala != null ? " | Sala: " + x.Harmonogram.Sala.Nazwa : ""),

                        UzytkownikNazwa =
                            ((x.Uzytkownik.Imie ?? "") + " " + (x.Uzytkownik.Nazwisko ?? "")).Trim()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<List<RejestracjaNaZajeciaDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            return await _context.RejestracjeNaZajecia
                .AsNoTracking()
                .Include(x => x.Harmonogram)
                    .ThenInclude(h => h.RodzajZajec)
                .Include(x => x.Harmonogram)
                    .ThenInclude(h => h.Klub)
                .Include(x => x.Harmonogram)
                    .ThenInclude(h => h.Trener)
                        .ThenInclude(t => t.Uzytkownik)
                .Where(x => x.UzytkownikId == uzytkownikId)
                .OrderByDescending(x => x.Harmonogram.StartAt)
                .Select(x => new RejestracjaNaZajeciaDto
                {
                    RejestracjaId = x.RejestracjaId,
                    HarmonogramId = x.HarmonogramId,
                    UzytkownikId = x.UzytkownikId,
                    Status = x.Status ?? string.Empty,
                    DataRejestracji = x.DataRejestracji,
                    Zrodlo = x.Zrodlo ?? string.Empty,

                    HarmonogramNazwa =
                        (x.Harmonogram.RodzajZajec != null ? x.Harmonogram.RodzajZajec.Nazwa : "Zajęcia")
                        + " | "
                        + x.Harmonogram.StartAt.ToString("dd.MM.yyyy HH:mm")
                        + " - "
                        + x.Harmonogram.EndAt.ToString("HH:mm")
                        + " | "
                        + (x.Harmonogram.Klub != null ? x.Harmonogram.Klub.Nazwa : "Brak klubu"),

                    UzytkownikNazwa = ""
                })
                .ToListAsync();
        }
        public async Task<RejestracjaNaZajeciaDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.RejestracjeNaZajecia
                    .AsNoTracking()
                    .Include(x => x.Harmonogram)
                        .ThenInclude(h => h.RodzajZajec)
                    .Include(x => x.Harmonogram)
                        .ThenInclude(h => h.Sala)
                    .Include(x => x.Uzytkownik)
                    .Where(x => x.RejestracjaId == id)
                    .Select(x => new RejestracjaNaZajeciaDto
                    {
                        RejestracjaId = x.RejestracjaId,
                        HarmonogramId = x.HarmonogramId,
                        UzytkownikId = x.UzytkownikId,
                        Status = x.Status ?? string.Empty,
                        DataRejestracji = x.DataRejestracji,
                        Zrodlo = x.Zrodlo ?? string.Empty,

                        HarmonogramNazwa =
                            (x.Harmonogram.RodzajZajec != null ? x.Harmonogram.RodzajZajec.Nazwa : "Zajęcia")
                            + " | "
                            + x.Harmonogram.StartAt.ToString("dd.MM.yyyy HH:mm")
                            + " - "
                            + x.Harmonogram.EndAt.ToString("HH:mm")
                            + (x.Harmonogram.Sala != null ? " | Sala: " + x.Harmonogram.Sala.Nazwa : ""),

                        UzytkownikNazwa =
                            ((x.Uzytkownik.Imie ?? "") + " " + (x.Uzytkownik.Nazwisko ?? "")).Trim()
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<RejestracjaNaZajeciaFormDataDto> GetFormDataAsync()
        {
            try
            {
                var result = new RejestracjaNaZajeciaFormDataDto();

                result.Harmonogramy = await _context.HarmonogramZajecs
    .AsNoTracking()
    .Where(x => x.StartAt > DateTime.Now)
    .Include(x => x.RodzajZajec)
    .Include(x => x.Sala)
    .OrderBy(x => x.StartAt)
    .Select(x => new LookupItemDto
    {
        Id = x.HarmonogramId,
        Nazwa =
            (x.RodzajZajec != null ? x.RodzajZajec.Nazwa : "Zajęcia")
            + " | "
            + x.StartAt.ToString("dd.MM.yyyy HH:mm")
            + " - "
            + x.EndAt.ToString("HH:mm")
            + (x.Sala != null ? " | Sala: " + x.Sala.Nazwa : "")
    })
    .ToListAsync();

                result.Uzytkownicy = await _context.Users
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwisko)
                    .ThenBy(x => x.Imie)
                    .Select(x => new LookupItemDto
                    {
                        Id = x.Id,
                        Nazwa = ((x.Imie ?? "") + " " + (x.Nazwisko ?? "")).Trim()
                    })
                    .ToListAsync();

                result.Statusy = new List<LookupItemDto>
                {
                    new LookupItemDto { Id = 1, Nazwa = "Nowa" },
                    new LookupItemDto { Id = 2, Nazwa = "Potwierdzona" },
                    new LookupItemDto { Id = 3, Nazwa = "Anulowana" },
                    new LookupItemDto { Id = 4, Nazwa = "Obecny" },
                    new LookupItemDto { Id = 5, Nazwa = "Nieobecny" }
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateRejestracjaNaZajeciaDto dto)
        {
            try
            {
                var harmonogram = await _context.HarmonogramZajecs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.HarmonogramId == dto.HarmonogramId);

                if (harmonogram == null)
                    return false;

                if (harmonogram.StartAt <= DateTime.Now)
                    return false;

                var liczbaZapisanych = await _context.RejestracjeNaZajecia
                    .CountAsync(x =>
                        x.HarmonogramId == dto.HarmonogramId &&
                        x.Status == "ZAPISANY");

                if (harmonogram.LimitMiejsc.HasValue &&
                    liczbaZapisanych >= harmonogram.LimitMiejsc.Value)
                {
                    return false;
                }

                var entity = new RejestracjeNaZajecium
                {
                    HarmonogramId = dto.HarmonogramId,
                    UzytkownikId = dto.UzytkownikId,
                    Status = string.IsNullOrWhiteSpace(dto.Status) ? "ZAPISANY" : dto.Status.Trim(),
                    DataRejestracji = dto.DataRejestracji ?? DateTime.Now,
                    Zrodlo = dto.Zrodlo?.Trim()
                };

                _context.RejestracjeNaZajecia.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateRejestracjaNaZajeciaDto dto)
        {
            try
            {
                var entity = await _context.RejestracjeNaZajecia
                    .FirstOrDefaultAsync(x => x.RejestracjaId == id);

                if (entity == null)
                    return false;

                entity.HarmonogramId = dto.HarmonogramId;
                entity.UzytkownikId = dto.UzytkownikId;
                entity.Status = string.IsNullOrWhiteSpace(dto.Status) ? "Nowa" : dto.Status.Trim();
                entity.DataRejestracji = dto.DataRejestracji ?? entity.DataRejestracji;
                entity.Zrodlo = dto.Zrodlo?.Trim();

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.RejestracjeNaZajecia
                    .FirstOrDefaultAsync(x => x.RejestracjaId == id);

                if (entity == null)
                    return false;

                _context.RejestracjeNaZajecia.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync RejestracjeNaZajecia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}