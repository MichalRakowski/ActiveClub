using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.PozycjeZamowienia;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class PozycjeZamowieniaService : IPozycjeZamowieniaService
    {
        private readonly ActiveClubContext _context;

        public PozycjeZamowieniaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<PozycjaZamowieniaDto>> GetAllAsync()
        {
            try
            {
                return await _context.PozycjeZamowienia
                    .AsNoTracking()
                    .Include(x => x.Zamowienie)
                    .OrderBy(x => x.PozycjaId)
                    .Select(x => new PozycjaZamowieniaDto
                    {
                        PozycjaId = x.PozycjaId,
                        ZamowienieId = x.ZamowienieId,
                        NumerZamowienia = x.Zamowienie != null ? x.Zamowienie.NumerZamowienia : string.Empty,
                        Typ = x.Typ,
                        RefId = x.RefId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        Ilosc = x.Ilosc,
                        CenaJedn = x.CenaJedn,
                        KwotaBrutto = x.KwotaBrutto
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PozycjaZamowieniaDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.PozycjeZamowienia
                    .AsNoTracking()
                    .Include(x => x.Zamowienie)
                    .Where(x => x.PozycjaId == id)
                    .Select(x => new PozycjaZamowieniaDto
                    {
                        PozycjaId = x.PozycjaId,
                        ZamowienieId = x.ZamowienieId,
                        NumerZamowienia = x.Zamowienie != null ? x.Zamowienie.NumerZamowienia : string.Empty,
                        Typ = x.Typ,
                        RefId = x.RefId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        Ilosc = x.Ilosc,
                        CenaJedn = x.CenaJedn,
                        KwotaBrutto = x.KwotaBrutto
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreatePozycjaZamowieniaDto dto)
        {
            try
            {
                var entity = new PozycjeZamowienium
                {
                    ZamowienieId = dto.ZamowienieId,
                    Typ = dto.Typ?.Trim() ?? string.Empty,
                    RefId = dto.RefId,
                    Nazwa = dto.Nazwa?.Trim() ?? string.Empty,
                    Opis = dto.Opis,
                    Ilosc = dto.Ilosc,
                    CenaJedn = dto.CenaJedn,
                    KwotaBrutto = dto.KwotaBrutto
                };

                _context.PozycjeZamowienia.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdatePozycjaZamowieniaDto dto)
        {
            try
            {
                var entity = await _context.PozycjeZamowienia
                    .FirstOrDefaultAsync(x => x.PozycjaId == id);

                if (entity == null)
                    return false;

                entity.ZamowienieId = dto.ZamowienieId;
                entity.Typ = dto.Typ?.Trim() ?? string.Empty;
                entity.RefId = dto.RefId;
                entity.Nazwa = dto.Nazwa?.Trim() ?? string.Empty;
                entity.Opis = dto.Opis;
                entity.Ilosc = dto.Ilosc;
                entity.CenaJedn = dto.CenaJedn;
                entity.KwotaBrutto = dto.KwotaBrutto;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.PozycjeZamowienia
                    .FirstOrDefaultAsync(x => x.PozycjaId == id);

                if (entity == null)
                    return false;

                _context.PozycjeZamowienia.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<PozycjaZamowieniaFormDataDto> GetFormDataAsync()
        {
            try
            {
                var dto = new PozycjaZamowieniaFormDataDto();

                dto.Zamowienia = await _context.Zamowienia
                    .AsNoTracking()
                    .OrderByDescending(x => x.DataUtworzenia)
                    .Select(x => new LookupItemDto
                    {
                        Id = x.ZamowienieId,
                        Nazwa = x.NumerZamowienia
                    })
                    .ToListAsync();

                dto.Typy = new List<LookupItemDto>
                {
                    new LookupItemDto { Id = 1, Nazwa = "Karnet" },
                    new LookupItemDto { Id = 2, Nazwa = "Produkt" },
                    new LookupItemDto { Id = 3, Nazwa = "Usługa" },
                    new LookupItemDto { Id = 4, Nazwa = "Członkostwo" },
                    new LookupItemDto { Id = 5, Nazwa = "Inne" }
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}