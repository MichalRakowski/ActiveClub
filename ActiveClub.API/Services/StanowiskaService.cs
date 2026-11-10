using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Stanowiska;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class StanowiskaService : IStanowiskaService
    {
        private readonly ActiveClubContext _context;

        public StanowiskaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<StanowiskoDto>> GetAllAsync()
        {
            try
            {
                return await _context.Stanowiskas
                    .AsNoTracking()
                    .OrderBy(x => x.Nazwa)
                    .Select(x => new StanowiskoDto
                    {
                        StanowiskoId = x.StanowiskoId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        CzyAktywne = x.CzyAktywne
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync Stanowiska:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<StanowiskoDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Stanowiskas
                    .AsNoTracking()
                    .Where(x => x.StanowiskoId == id)
                    .Select(x => new StanowiskoDto
                    {
                        StanowiskoId = x.StanowiskoId,
                        Nazwa = x.Nazwa,
                        Opis = x.Opis,
                        CzyAktywne = x.CzyAktywne
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync Stanowiska:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateStanowiskoDto dto)
        {
            try
            {
                var entity = new Stanowiska
                {
                    Nazwa = dto.Nazwa?.Trim() ?? string.Empty,
                    Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim(),
                    CzyAktywne = dto.CzyAktywne
                };

                _context.Stanowiskas.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync Stanowiska:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateStanowiskoDto dto)
        {
            try
            {
                var entity = await _context.Stanowiskas
                    .FirstOrDefaultAsync(x => x.StanowiskoId == id);

                if (entity == null) return false;

                entity.Nazwa = dto.Nazwa?.Trim() ?? string.Empty;
                entity.Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim();
                entity.CzyAktywne = dto.CzyAktywne;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync Stanowiska:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Stanowiskas
                    .FirstOrDefaultAsync(x => x.StanowiskoId == id);

                if (entity == null) return false;

                _context.Stanowiskas.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync Stanowiska:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}