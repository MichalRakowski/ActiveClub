using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.PracownicyKlubu;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class PracownicyKlubuService : IPracownicyKlubuService
    {
        private readonly ActiveClubContext _context;

        public PracownicyKlubuService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<PracownikKlubuDto>> GetAllAsync()
        {
            try
            {
                return await _context.PracownicyKlubus
                    .AsNoTracking()
                    .Include(x => x.Uzytkownik)
                    .Include(x => x.Klub)
                    .Include(x => x.Stanowisko)
                    .OrderBy(x => x.PracownikKlubId)
                    .Select(x => new PracownikKlubuDto
                    {
                        PracownikKlubId = x.PracownikKlubId,
                        UzytkownikId = x.UzytkownikId,
                        KlubId = x.KlubId,
                        StanowiskoId = x.StanowiskoId,
                        UzytkownikNazwa = x.Uzytkownik != null
                            ? ((x.Uzytkownik.Imie ?? "") + " " + (x.Uzytkownik.Nazwisko ?? "")).Trim()
                            : "",
                        KlubNazwa = x.Klub != null ? x.Klub.Nazwa : "",
                        StanowiskoNazwa = x.Stanowisko != null ? x.Stanowisko.Nazwa : "",
                        DataOd = x.DataOd.HasValue ? x.DataOd.Value.ToDateTime(TimeOnly.MinValue) : null,
                        DataDo = x.DataDo.HasValue ? x.DataDo.Value.ToDateTime(TimeOnly.MinValue) : null,
                        CzyAktywny = x.CzyAktywny ?? false,
                        Uwagi = x.Uwagi
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PracownikKlubuDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.PracownicyKlubus
                    .AsNoTracking()
                    .Include(x => x.Uzytkownik)
                    .Include(x => x.Klub)
                    .Include(x => x.Stanowisko)
                    .Where(x => x.PracownikKlubId == id)
                    .Select(x => new PracownikKlubuDto
                    {
                        PracownikKlubId = x.PracownikKlubId,
                        UzytkownikId = x.UzytkownikId,
                        KlubId = x.KlubId,
                        StanowiskoId = x.StanowiskoId,
                        UzytkownikNazwa = x.Uzytkownik != null
                            ? ((x.Uzytkownik.Imie ?? "") + " " + (x.Uzytkownik.Nazwisko ?? "")).Trim()
                            : "",
                        KlubNazwa = x.Klub != null ? x.Klub.Nazwa : "",
                        StanowiskoNazwa = x.Stanowisko != null ? x.Stanowisko.Nazwa : "",
                        DataOd = x.DataOd.HasValue ? x.DataOd.Value.ToDateTime(TimeOnly.MinValue) : null,
                        DataDo = x.DataDo.HasValue ? x.DataDo.Value.ToDateTime(TimeOnly.MinValue) : null,
                        CzyAktywny = x.CzyAktywny ?? false,
                        Uwagi = x.Uwagi
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PracownikKlubuFormDataDto> GetFormDataAsync()
        {
            try
            {
                var dto = new PracownikKlubuFormDataDto
                {
                    Uzytkownicy = await _context.Users
                        .AsNoTracking()
                        .OrderBy(x => x.Imie)
                        .ThenBy(x => x.Nazwisko)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.Id,
                            Nazwa = ((x.Imie ?? "") + " " + (x.Nazwisko ?? "")).Trim()
                        })
                        .ToListAsync(),

                    Kluby = await _context.Klubies
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.KlubId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync(),

                    Stanowiska = await _context.Stanowiskas
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.StanowiskoId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync()
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreatePracownikKlubuDto dto)
        {
            try
            {
                var entity = new PracownicyKlubu
                {
                    UzytkownikId = dto.UzytkownikId,
                    KlubId = dto.KlubId,
                    StanowiskoId = dto.StanowiskoId,
                    DataOd = dto.DataOd.HasValue ? DateOnly.FromDateTime(dto.DataOd.Value) : null,
                    DataDo = dto.DataDo.HasValue ? DateOnly.FromDateTime(dto.DataDo.Value) : null,
                    CzyAktywny = dto.CzyAktywny,
                    Uwagi = dto.Uwagi?.Trim()
                };

                _context.PracownicyKlubus.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdatePracownikKlubuDto dto)
        {
            try
            {
                var entity = await _context.PracownicyKlubus
                    .FirstOrDefaultAsync(x => x.PracownikKlubId == id);

                if (entity == null)
                    return false;

                entity.UzytkownikId = dto.UzytkownikId;
                entity.KlubId = dto.KlubId;
                entity.StanowiskoId = dto.StanowiskoId;
                entity.DataOd = dto.DataOd.HasValue ? DateOnly.FromDateTime(dto.DataOd.Value) : null;
                entity.DataDo = dto.DataDo.HasValue ? DateOnly.FromDateTime(dto.DataDo.Value) : null;
                entity.CzyAktywny = dto.CzyAktywny;
                entity.Uwagi = dto.Uwagi?.Trim();

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.PracownicyKlubus
                    .FirstOrDefaultAsync(x => x.PracownikKlubId == id);

                if (entity == null)
                    return false;

                _context.PracownicyKlubus.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}