using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.Platnosci;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class PlatnosciService : IPlatnosciService
    {
        private readonly ActiveClubContext _context;

        public PlatnosciService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<PlatnoscDto>> GetAllAsync()
        {
            try
            {
                return await _context.Platnoscis
                    .AsNoTracking()
                    .Include(x => x.Zamowienie)
                    .Include(x => x.MetodaPlatnosci)
                    .OrderByDescending(x => x.DataUtworzenia)
                    .Select(x => new PlatnoscDto
                    {
                        PlatnoscId = x.PlatnoscId,
                        ZamowienieId = x.ZamowienieId,
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        Kwota = x.Kwota ?? 0,
                        Waluta = x.Waluta ?? string.Empty,
                        Status = x.Status ?? string.Empty,
                        Provider = x.Provider ?? string.Empty,
                        ProviderTransactionId = x.ProviderTransactionId ?? string.Empty,
                        DataPlatnosci = x.DataPlatnosci,
                        DataUtworzenia = x.DataUtworzenia,
                        Uwagi = x.Uwagi ?? string.Empty,
                        ZamowienieDisplay = x.Zamowienie != null
                            ? $"{x.Zamowienie.NumerZamowienia} | {x.Zamowienie.SumaBrutto} {x.Zamowienie.Waluta}"
                            : $"Zamówienie ID: {x.ZamowienieId}",
                        MetodaPlatnosciDisplay = x.MetodaPlatnosci != null
                            ? x.MetodaPlatnosci.Nazwa
                            : $"Metoda ID: {x.MetodaPlatnosciId}"
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PlatnoscDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Platnoscis
                    .AsNoTracking()
                    .Include(x => x.Zamowienie)
                    .Include(x => x.MetodaPlatnosci)
                    .Where(x => x.PlatnoscId == id)
                    .Select(x => new PlatnoscDto
                    {
                        PlatnoscId = x.PlatnoscId,
                        ZamowienieId = x.ZamowienieId,
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        Kwota = x.Kwota ?? 0,
                        Waluta = x.Waluta ?? string.Empty,
                        Status = x.Status ?? string.Empty,
                        Provider = x.Provider ?? string.Empty,
                        ProviderTransactionId = x.ProviderTransactionId ?? string.Empty,
                        DataPlatnosci = x.DataPlatnosci,
                        DataUtworzenia = x.DataUtworzenia,
                        Uwagi = x.Uwagi ?? string.Empty,
                        ZamowienieDisplay = x.Zamowienie != null
                            ? $"{x.Zamowienie.NumerZamowienia} | {x.Zamowienie.SumaBrutto} {x.Zamowienie.Waluta}"
                            : $"Zamówienie ID: {x.ZamowienieId}",
                        MetodaPlatnosciDisplay = x.MetodaPlatnosci != null
                            ? x.MetodaPlatnosci.Nazwa
                            : $"Metoda ID: {x.MetodaPlatnosciId}"
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreatePlatnoscDto dto)
        {
            try
            {
                var entity = new Platnosci
                {
                    ZamowienieId = dto.ZamowienieId,
                    MetodaPlatnosciId = dto.MetodaPlatnosciId,
                    Kwota = dto.Kwota,
                    Waluta = dto.Waluta?.Trim(),
                    Status = dto.Status?.Trim(),
                    Provider = dto.Provider?.Trim(),
                    ProviderTransactionId = dto.ProviderTransactionId?.Trim(),
                    DataPlatnosci = dto.DataPlatnosci,
                    DataUtworzenia = dto.DataUtworzenia ?? DateTime.Now,
                    Uwagi = dto.Uwagi?.Trim()
                };

                _context.Platnoscis.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdatePlatnoscDto dto)
        {
            try
            {
                var entity = await _context.Platnoscis
                    .FirstOrDefaultAsync(x => x.PlatnoscId == id);

                if (entity == null)
                    return false;

                entity.ZamowienieId = dto.ZamowienieId;
                entity.MetodaPlatnosciId = dto.MetodaPlatnosciId;
                entity.Kwota = dto.Kwota;
                entity.Waluta = dto.Waluta?.Trim();
                entity.Status = dto.Status?.Trim();
                entity.Provider = dto.Provider?.Trim();
                entity.ProviderTransactionId = dto.ProviderTransactionId?.Trim();
                entity.DataPlatnosci = dto.DataPlatnosci;
                entity.DataUtworzenia = dto.DataUtworzenia;
                entity.Uwagi = dto.Uwagi?.Trim();

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Platnoscis
                    .FirstOrDefaultAsync(x => x.PlatnoscId == id);

                if (entity == null)
                    return false;

                _context.Platnoscis.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PlatnoscFormDataDto> GetFormDataAsync()
        {
            try
            {
                var dto = new PlatnoscFormDataDto
                {
                    Zamowienia = await _context.Zamowienia
                        .AsNoTracking()
                        .OrderByDescending(x => x.DataUtworzenia)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.ZamowienieId,
                            Nazwa = $"{x.NumerZamowienia} | {x.SumaBrutto} {x.Waluta}"
                        })
                        .ToListAsync(),

                    MetodyPlatnosci = await _context.MetodyPlatnoscis
                        .AsNoTracking()
                        .Where(x => x.CzyAktywna)
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.MetodaPlatnosciId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync(),

                    Statusy = new List<LookupItemDto>
                    {
                        new LookupItemDto { Id = 1, Nazwa = "Nowa" },
                        new LookupItemDto { Id = 2, Nazwa = "Oczekująca" },
                        new LookupItemDto { Id = 3, Nazwa = "Opłacona" },
                        new LookupItemDto { Id = 4, Nazwa = "Anulowana" },
                        new LookupItemDto { Id = 5, Nazwa = "Odrzucona" },
                        new LookupItemDto { Id = 6, Nazwa = "Zwrócona" }
                    },

                    Waluty = new List<LookupItemDto>
                    {
                        new LookupItemDto { Id = 1, Nazwa = "PLN" },
                        new LookupItemDto { Id = 2, Nazwa = "EUR" },
                        new LookupItemDto { Id = 3, Nazwa = "USD" }
                    }
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync Platnosci:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}