using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Zamowienia;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class ZamowieniaService : IZamowieniaService
    {
        private readonly ActiveClubContext _context;

        public ZamowieniaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<ZamowienieDto>> GetAllAsync()
        {
            try
            {
                return await _context.Zamowienia
                    .AsNoTracking()
                    .OrderByDescending(x => x.DataUtworzenia)
                    .Select(x => new ZamowienieDto
                    {
                        ZamowienieId = x.ZamowienieId,
                        NumerZamowienia = x.NumerZamowienia,
                        UzytkownikId = x.UzytkownikId,
                        KlubId = x.KlubId,
                        Status = x.Status,
                        SumaBrutto = x.SumaBrutto,
                        KwotaRabatu = x.KwotaRabatu,
                        Waluta = x.Waluta,
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        RabatId = x.RabatId,
                        TypKanalu = x.TypKanalu,
                        DataUtworzenia = x.DataUtworzenia,
                        DataOplacenia = x.DataOplacenia,
                        UtworzylUzytkownikId = x.UtworzylUzytkownikId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<ZamowienieDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Zamowienia
                    .AsNoTracking()
                    .Where(x => x.ZamowienieId == id)
                    .Select(x => new ZamowienieDto
                    {
                        ZamowienieId = x.ZamowienieId,
                        NumerZamowienia = x.NumerZamowienia,
                        UzytkownikId = x.UzytkownikId,
                        KlubId = x.KlubId,
                        Status = x.Status,
                        SumaBrutto = x.SumaBrutto,
                        KwotaRabatu = x.KwotaRabatu,
                        Waluta = x.Waluta,
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        RabatId = x.RabatId,
                        TypKanalu = x.TypKanalu,
                        DataUtworzenia = x.DataUtworzenia,
                        DataOplacenia = x.DataOplacenia,
                        UtworzylUzytkownikId = x.UtworzylUzytkownikId
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateZamowienieDto dto)
        {
            try
            {
                Console.WriteLine("WESZŁO DO ZamowieniaService.CreateAsync");
                var entity = new Zamowienium
                {
                    NumerZamowienia = dto.NumerZamowienia?.Trim() ?? string.Empty,
                    UzytkownikId = dto.UzytkownikId,
                    KlubId = dto.KlubId,
                    Status = dto.Status?.Trim() ?? string.Empty,
                    SumaBrutto = dto.SumaBrutto,
                    KwotaRabatu = dto.KwotaRabatu,
                    Waluta = dto.Waluta?.Trim() ?? "PLN",
                    MetodaPlatnosciId = dto.MetodaPlatnosciId,
                    RabatId = dto.RabatId,
                    TypKanalu = dto.TypKanalu?.Trim() ?? string.Empty,
                    DataUtworzenia = dto.DataUtworzenia,
                    DataOplacenia = dto.DataOplacenia,
                    UtworzylUzytkownikId = dto.UtworzylUzytkownikId
                };

                _context.Zamowienia.Add(entity);
                await _context.SaveChangesAsync();

                var status = entity.Status?.Trim();

                var czyOplacone =
                    string.Equals(status, "OPLACONE", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "OPŁACONE", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "Opłacone", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "Opłacona", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(status, "ZREALIZOWANE", StringComparison.OrdinalIgnoreCase) ||
string.Equals(status, "Zrealizowane", StringComparison.OrdinalIgnoreCase) ||
                    entity.DataOplacenia.HasValue;

                if (czyOplacone)
                {
                    Console.WriteLine("TWORZĘ PŁATNOŚĆ");
                    var platnosc = new Platnosci
                    {
                        ZamowienieId = entity.ZamowienieId,
                        MetodaPlatnosciId = entity.MetodaPlatnosciId ?? 1,
                        Kwota = entity.SumaBrutto,
                        Waluta = entity.Waluta ?? "PLN",
                        Status = "Opłacona",
                        Provider = "System",
                        ProviderTransactionId = null,
                        DataPlatnosci = entity.DataOplacenia ?? DateTime.Now,
                        DataUtworzenia = DateTime.Now,
                        Uwagi = "Płatność utworzona automatycznie po opłaceniu zamówienia."
                    };

                    _context.Platnoscis.Add(platnosc);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateZamowienieDto dto)
        {
            try
            {
                var entity = await _context.Zamowienia
                    .FirstOrDefaultAsync(x => x.ZamowienieId == id);

                if (entity == null)
                    return false;

                entity.NumerZamowienia = dto.NumerZamowienia?.Trim() ?? string.Empty;
                entity.UzytkownikId = dto.UzytkownikId;
                entity.KlubId = dto.KlubId;
                entity.Status = dto.Status?.Trim() ?? string.Empty;
                entity.SumaBrutto = dto.SumaBrutto;
                entity.KwotaRabatu = dto.KwotaRabatu;
                entity.Waluta = dto.Waluta?.Trim() ?? "PLN";
                entity.MetodaPlatnosciId = dto.MetodaPlatnosciId;
                entity.RabatId = dto.RabatId;
                entity.TypKanalu = dto.TypKanalu?.Trim() ?? string.Empty;
                entity.DataUtworzenia = dto.DataUtworzenia;
                entity.DataOplacenia = dto.DataOplacenia;
                entity.UtworzylUzytkownikId = dto.UtworzylUzytkownikId;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Zamowienia
                    .FirstOrDefaultAsync(x => x.ZamowienieId == id);

                if (entity == null)
                    return false;

                _context.Zamowienia.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<ZamowienieFormDataDto> GetFormDataAsync()
        {
            try
            {
                var result = new ZamowienieFormDataDto
                {
                    Uzytkownicy = await _context.Users
                        .AsNoTracking()
                        .OrderBy(x => x.UserName)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.Id,
                            Nazwa = x.UserName ?? x.Email ?? ("Użytkownik " + x.Id)
                        })
                        .ToListAsync(),

                    UtworzylUzytkownicy = await _context.Users
                        .AsNoTracking()
                        .OrderBy(x => x.UserName)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.Id,
                            Nazwa = x.UserName ?? x.Email ?? ("Użytkownik " + x.Id)
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

                    MetodyPlatnosci = await _context.MetodyPlatnoscis
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.MetodaPlatnosciId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync(),

                    Rabaty = await _context.Rabaties
                        .AsNoTracking()
                        .OrderBy(x => x.Kod)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.RabatId,
                            Nazwa = x.Kod
                        })
                        .ToListAsync()
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync Zamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}