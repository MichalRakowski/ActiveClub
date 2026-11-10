using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.MetodyPlatnosci;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class MetodyPlatnosciService : IMetodyPlatnosciService
    {
        private readonly ActiveClubContext _context;

        public MetodyPlatnosciService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<MetodaPlatnosciDto>> GetAllAsync()
        {
            try
            {
                return await _context.MetodyPlatnoscis
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new MetodaPlatnosciDto
                    {
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        Nazwa = x.Nazwa,
                        CzyAktywna = x.CzyAktywna
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<MetodaPlatnosciDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.MetodyPlatnoscis
                    .AsNoTracking()
                    .Where(x => x.MetodaPlatnosciId == id)
                    .Select(x => new MetodaPlatnosciDto
                    {
                        MetodaPlatnosciId = x.MetodaPlatnosciId,
                        Nazwa = x.Nazwa,
                        CzyAktywna = x.CzyAktywna
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateMetodaPlatnosciDto dto)
        {
            try
            {
                var entity = new MetodyPlatnosci
                {
                    Nazwa = dto.Nazwa?.Trim() ?? string.Empty,
                    CzyAktywna = dto.CzyAktywna
                };

                _context.MetodyPlatnoscis.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateMetodaPlatnosciDto dto)
        {
            try
            {
                var entity = await _context.MetodyPlatnoscis
                    .FirstOrDefaultAsync(x => x.MetodaPlatnosciId == id);

                if (entity == null)
                    return false;

                entity.Nazwa = dto.Nazwa?.Trim() ?? string.Empty;
                entity.CzyAktywna = dto.CzyAktywna;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.MetodyPlatnoscis
                    .FirstOrDefaultAsync(x => x.MetodaPlatnosciId == id);

                if (entity == null)
                    return false;

                _context.MetodyPlatnoscis.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}