using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Rabaty;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class RabatyService : IRabatyService
    {
        private readonly ActiveClubContext _context;

        public RabatyService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<RabatDto>> GetAllAsync()
        {
            try
            {
                return await _context.Rabaties
                    .AsNoTracking()
                    .OrderBy(x => x.Kod)
                    .Select(x => new RabatDto
                    {
                        RabatId = x.RabatId,
                        Kod = x.Kod,
                        Typ = x.Typ,
                        Wartosc = x.Wartosc,
                        DataOd = x.DataOd,
                        DataDo = x.DataDo,
                        MinimalnaKwotaZamowienia = x.MinimalnaKwotaZamowienia,
                        MaksLiczbaUzyc = x.MaksLiczbaUzyc,
                        CzyAktywny = x.CzyAktywny
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Rabaty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<RabatDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Rabaties
                    .AsNoTracking()
                    .Where(x => x.RabatId == id)
                    .Select(x => new RabatDto
                    {
                        RabatId = x.RabatId,
                        Kod = x.Kod,
                        Typ = x.Typ,
                        Wartosc = x.Wartosc,
                        DataOd = x.DataOd,
                        DataDo = x.DataDo,
                        MinimalnaKwotaZamowienia = x.MinimalnaKwotaZamowienia,
                        MaksLiczbaUzyc = x.MaksLiczbaUzyc,
                        CzyAktywny = x.CzyAktywny
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Rabaty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateRabatDto dto)
        {
            try
            {
                var entity = new Rabaty
                {
                    Kod = dto.Kod?.Trim() ?? string.Empty,
                    Typ = dto.Typ?.Trim() ?? string.Empty,
                    Wartosc = dto.Wartosc,
                    DataOd = dto.DataOd,
                    DataDo = dto.DataDo,
                    MinimalnaKwotaZamowienia = dto.MinimalnaKwotaZamowienia,
                    MaksLiczbaUzyc = dto.MaksLiczbaUzyc,
                    CzyAktywny = dto.CzyAktywny
                };

                _context.Rabaties.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync Rabaty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateRabatDto dto)
        {
            try
            {
                var entity = await _context.Rabaties
                    .FirstOrDefaultAsync(x => x.RabatId == id);

                if (entity == null)
                    return false;

                entity.Kod = dto.Kod?.Trim() ?? string.Empty;
                entity.Typ = dto.Typ?.Trim() ?? string.Empty;
                entity.Wartosc = dto.Wartosc;
                entity.DataOd = dto.DataOd;
                entity.DataDo = dto.DataDo;
                entity.MinimalnaKwotaZamowienia = dto.MinimalnaKwotaZamowienia;
                entity.MaksLiczbaUzyc = dto.MaksLiczbaUzyc;
                entity.CzyAktywny = dto.CzyAktywny;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Rabaty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Rabaties
                    .FirstOrDefaultAsync(x => x.RabatId == id);

                if (entity == null)
                    return false;

                _context.Rabaties.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Rabaty:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}