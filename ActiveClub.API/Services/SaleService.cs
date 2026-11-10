using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Sale;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly ActiveClubContext _context;

        public SaleService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<SalaDto>> GetAllAsync()
        {
            try
            {
                return await _context.Sales
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new SalaDto
                    {
                        SalaId = x.SalaId,
                        KlubId = x.KlubId,
                        KlubNazwa = x.Klub.Nazwa,
                        Nazwa = x.Nazwa,
                        Pojemnosc = x.Pojemnosc,
                        Opis = x.Opis,
                        CzyAktywna = x.CzyAktywna
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Sale:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<SalaDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Sales
                    .AsNoTracking()
                    .Where(x => x.SalaId == id)
                    .Select(x => new SalaDto
                    {
                        SalaId = x.SalaId,
                        KlubId = x.KlubId,
                        KlubNazwa = x.Klub.Nazwa,
                        Nazwa = x.Nazwa,
                        Pojemnosc = x.Pojemnosc,
                        Opis = x.Opis,
                        CzyAktywna = x.CzyAktywna
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Sale:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateSalaDto dto)
        {
            try
            {
                var klubExists = await _context.Klubies.AnyAsync(x => x.KlubId == dto.KlubId);
                if (!klubExists)
                    return false;

                var entity = new Sale
                {
                    KlubId = dto.KlubId,
                    Nazwa = dto.Nazwa?.Trim() ?? string.Empty,
                    Pojemnosc = dto.Pojemnosc,
                    Opis = dto.Opis,
                    CzyAktywna = dto.CzyAktywna
                };

                _context.Sales.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync Sale:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateSalaDto dto)
        {
            try
            {
                var entity = await _context.Sales
                    .FirstOrDefaultAsync(x => x.SalaId == id);

                if (entity == null)
                    return false;

                var klubExists = await _context.Klubies.AnyAsync(x => x.KlubId == dto.KlubId);
                if (!klubExists)
                    return false;

                entity.KlubId = dto.KlubId;
                entity.Nazwa = dto.Nazwa?.Trim() ?? string.Empty;
                entity.Pojemnosc = dto.Pojemnosc;
                entity.Opis = dto.Opis;
                entity.CzyAktywna = dto.CzyAktywna;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Sale:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Sales
                    .FirstOrDefaultAsync(x => x.SalaId == id);

                if (entity == null)
                    return false;

                _context.Sales.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Sale:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}