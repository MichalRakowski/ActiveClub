using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Opinie;
using ActiveClubData.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class OpinieService : IOpinieService
    {
        private readonly ActiveClubContext _context;

        public OpinieService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<OpiniaDto>> GetAllAsync()
        {
            try
            {
                return await _context.Opinies
                    .AsNoTracking()
                    .OrderByDescending(x => x.DataDodania)
                    .Select(x => new OpiniaDto
                    {
                        OpiniaId = x.OpiniaId,
                        KlubId = x.KlubId,
                        UzytkownikId = x.UzytkownikId,
                        Ocena = x.Ocena,
                        Komentarz = x.Komentarz,
                        CzyWidoczna = x.CzyWidoczna,
                        DataDodania = x.DataDodania
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Opinie:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<OpiniaDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Opinies
                    .AsNoTracking()
                    .Where(x => x.OpiniaId == id)
                    .Select(x => new OpiniaDto
                    {
                        OpiniaId = x.OpiniaId,
                        KlubId = x.KlubId,
                        UzytkownikId = x.UzytkownikId,
                        Ocena = x.Ocena,
                        Komentarz = x.Komentarz,
                        CzyWidoczna = x.CzyWidoczna,
                        DataDodania = x.DataDodania
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Opinie:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateOpiniaDto dto)
        {
            try
            {
                var entity = await _context.Opinies
                    .FirstOrDefaultAsync(x => x.OpiniaId == id);

                if (entity == null)
                    return false;

                entity.KlubId = dto.KlubId;
                entity.UzytkownikId = dto.UzytkownikId;
                entity.Ocena = dto.Ocena;
                entity.Komentarz = dto.Komentarz?.Trim();
                entity.CzyWidoczna = dto.CzyWidoczna;
                entity.DataDodania = dto.DataDodania;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Opinie:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Opinies
                    .FirstOrDefaultAsync(x => x.OpiniaId == id);

                if (entity == null)
                    return false;

                _context.Opinies.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Opinie:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}