using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.RodzajeZajec;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class RodzajeZajecService : IRodzajeZajecService
    {
        private readonly ActiveClubContext _context;

        public RodzajeZajecService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<RodzajZajecDto>> GetAllAsync()
        {
            try
            {
                return await _context.RodzajeZajecs
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new RodzajZajecDto
                    {
                        RodzajZajecId = x.RodzajZajecId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        CzasTrwaniaMin = x.CzasTrwaniaMin,
                        CzyAktywne = x.CzyAktywne,
                        CzyWidoczneOnline = x.CzyWidoczneOnline
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAll RodzajeZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<RodzajZajecDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.RodzajeZajecs
                    .AsNoTracking()
                    .Where(x => x.RodzajZajecId == id)
                    .Select(x => new RodzajZajecDto
                    {
                        RodzajZajecId = x.RodzajZajecId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        CzasTrwaniaMin = x.CzasTrwaniaMin,
                        CzyAktywne = x.CzyAktywne,
                        CzyWidoczneOnline = x.CzyWidoczneOnline
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetById RodzajeZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateRodzajZajecDto dto)
        {
            try
            {
                var entity = new RodzajeZajec
                {
                    Nazwa = dto.Nazwa.Trim(),
                    Opis = dto.Opis,
                    CzasTrwaniaMin = dto.CzasTrwaniaMin,
                    CzyAktywne = dto.CzyAktywne,
                    CzyWidoczneOnline = dto.CzyWidoczneOnline
                };

                _context.RodzajeZajecs.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE Create RodzajeZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateRodzajZajecDto dto)
        {
            try
            {
                var entity = await _context.RodzajeZajecs
                    .FirstOrDefaultAsync(x => x.RodzajZajecId == id);

                if (entity == null)
                    return false;

                entity.Nazwa = dto.Nazwa.Trim();
                entity.Opis = dto.Opis;
                entity.CzasTrwaniaMin = dto.CzasTrwaniaMin;
                entity.CzyAktywne = dto.CzyAktywne;
                entity.CzyWidoczneOnline = dto.CzyWidoczneOnline;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE Update RodzajeZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.RodzajeZajecs
                    .FirstOrDefaultAsync(x => x.RodzajZajecId == id);

                if (entity == null)
                    return false;

                _context.RodzajeZajecs.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE Delete RodzajeZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}