using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.GodzinyOtwarcia;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class GodzinyOtwarciaService : IGodzinyOtwarciaService
    {
        private readonly ActiveClubContext _context;

        public GodzinyOtwarciaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<GodzinyOtwarciaDto>> GetAllAsync()
        {
            return await _context.GodzinyOtwarcia
                .AsNoTracking()
                .OrderBy(x => x.KlubId)
                .ThenBy(x => x.DzienTygodnia)
                .Select(x => new GodzinyOtwarciaDto
                {
                    GodzinyOtwarciaId = x.GodzinyOtwarciaId,
                    KlubId = x.KlubId,
                    DzienTygodnia = x.DzienTygodnia,
                    GodzinaOd = x.GodzinaOd.HasValue ? x.GodzinaOd.Value.ToString("HH:mm") : null,
                    GodzinaDo = x.GodzinaDo.HasValue ? x.GodzinaDo.Value.ToString("HH:mm") : null,
                    CzyZamkniete = x.CzyZamkniete
                })
                .ToListAsync();
        }

        public async Task<List<GodzinyOtwarciaDto>> GetByKlubIdAsync(int klubId)
        {
            return await _context.GodzinyOtwarcia
                .AsNoTracking()
                .Where(x => x.KlubId == klubId)
                .OrderBy(x => x.DzienTygodnia)
                .Select(x => new GodzinyOtwarciaDto
                {
                    GodzinyOtwarciaId = x.GodzinyOtwarciaId,
                    KlubId = x.KlubId,
                    DzienTygodnia = x.DzienTygodnia,
                    GodzinaOd = x.GodzinaOd.HasValue ? x.GodzinaOd.Value.ToString("HH:mm") : null,
                    GodzinaDo = x.GodzinaDo.HasValue ? x.GodzinaDo.Value.ToString("HH:mm") : null,
                    CzyZamkniete = x.CzyZamkniete
                })
                .ToListAsync();
        }

        public async Task<GodzinyOtwarciaDto?> GetByIdAsync(int id)
        {
            return await _context.GodzinyOtwarcia
                .AsNoTracking()
                .Where(x => x.GodzinyOtwarciaId == id)
                .Select(x => new GodzinyOtwarciaDto
                {
                    GodzinyOtwarciaId = x.GodzinyOtwarciaId,
                    KlubId = x.KlubId,
                    DzienTygodnia = x.DzienTygodnia,
                    GodzinaOd = x.GodzinaOd.HasValue ? x.GodzinaOd.Value.ToString("HH:mm") : null,
                    GodzinaDo = x.GodzinaDo.HasValue ? x.GodzinaDo.Value.ToString("HH:mm") : null,
                    CzyZamkniete = x.CzyZamkniete
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(CreateGodzinyOtwarciaDto dto)
        {
            var entity = new GodzinyOtwarcium
            {
                KlubId = dto.KlubId,
                DzienTygodnia = dto.DzienTygodnia,
                GodzinaOd = dto.CzyZamkniete || string.IsNullOrWhiteSpace(dto.GodzinaOd)
                    ? null
                    : TimeOnly.Parse(dto.GodzinaOd),
                GodzinaDo = dto.CzyZamkniete || string.IsNullOrWhiteSpace(dto.GodzinaDo)
                    ? null
                    : TimeOnly.Parse(dto.GodzinaDo),
                CzyZamkniete = dto.CzyZamkniete
            };

            _context.GodzinyOtwarcia.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(int id, UpdateGodzinyOtwarciaDto dto)
        {
            var entity = await _context.GodzinyOtwarcia
                .FirstOrDefaultAsync(x => x.GodzinyOtwarciaId == id);

            if (entity == null)
                return false;

            entity.KlubId = dto.KlubId;
            entity.DzienTygodnia = dto.DzienTygodnia;
            entity.CzyZamkniete = dto.CzyZamkniete;
            entity.GodzinaOd = dto.CzyZamkniete || string.IsNullOrWhiteSpace(dto.GodzinaOd)
                ? null
                : TimeOnly.Parse(dto.GodzinaOd);
            entity.GodzinaDo = dto.CzyZamkniete || string.IsNullOrWhiteSpace(dto.GodzinaDo)
                ? null
                : TimeOnly.Parse(dto.GodzinaDo);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.GodzinyOtwarcia
                .FirstOrDefaultAsync(x => x.GodzinyOtwarciaId == id);

            if (entity == null)
                return false;

            _context.GodzinyOtwarcia.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}