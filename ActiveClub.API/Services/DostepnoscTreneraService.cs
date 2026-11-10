using ActiveClub.API.Data;
using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.DostepnoscTrenera;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class DostepnoscTreneraService : IDostepnoscTreneraService
    {
        private readonly ActiveClubContext _context;

        public DostepnoscTreneraService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DostepnoscTreneraDto>> GetAllAsync()
        {
            return await _context.DostepnoscTreneras
                .OrderByDescending(x => x.StartAt)
                .Select(x => new DostepnoscTreneraDto
                {
                    DostepnoscId = x.DostepnoscId,
                    TrenerId = x.TrenerId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    UtworzonoAt = x.UtworzonoAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DostepnoscTreneraDto>> GetByTrenerIdAsync(int trenerId)
        {
            return await _context.DostepnoscTreneras
                .Where(x => x.TrenerId == trenerId)
                .OrderByDescending(x => x.StartAt)
                .Select(x => new DostepnoscTreneraDto
                {
                    DostepnoscId = x.DostepnoscId,
                    TrenerId = x.TrenerId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    UtworzonoAt = x.UtworzonoAt
                })
                .ToListAsync();
        }

        public async Task<DostepnoscTreneraDto?> GetByIdAsync(int dostepnoscId)
        {
            return await _context.DostepnoscTreneras
                .Where(x => x.DostepnoscId == dostepnoscId)
                .Select(x => new DostepnoscTreneraDto
                {
                    DostepnoscId = x.DostepnoscId,
                    TrenerId = x.TrenerId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    UtworzonoAt = x.UtworzonoAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DostepnoscTreneraDto> CreateAsync(CreateDostepnoscTreneraDto dto)
        {
            Validate(dto.TrenerId, dto.StartAt, dto.EndAt);

            var hasConflict = await _context.DostepnoscTreneras.AnyAsync(x =>
                x.TrenerId == dto.TrenerId &&
                x.StartAt < dto.EndAt &&
                dto.StartAt < x.EndAt);

            if (hasConflict)
                throw new InvalidOperationException("Podany przedział czasu nakłada się na istniejącą dostępność trenera.");

            var entity = new DostepnoscTrenera
            {
                TrenerId = dto.TrenerId,
                KlubId = dto.KlubId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status,
                UtworzonoAt = DateTime.UtcNow
            };

            _context.DostepnoscTreneras.Add(entity);
            await _context.SaveChangesAsync();

            return new DostepnoscTreneraDto
            {
                DostepnoscId = entity.DostepnoscId,
                TrenerId = entity.TrenerId,
                KlubId = entity.KlubId,
                StartAt = entity.StartAt,
                EndAt = entity.EndAt,
                Status = entity.Status,
                UtworzonoAt = entity.UtworzonoAt
            };
        }

        public async Task<bool> UpdateAsync(int dostepnoscId, UpdateDostepnoscTreneraDto dto)
        {
            Validate(dto.TrenerId, dto.StartAt, dto.EndAt);

            var entity = await _context.DostepnoscTreneras
                .FirstOrDefaultAsync(x => x.DostepnoscId == dostepnoscId);

            if (entity == null)
                return false;

            var hasConflict = await _context.DostepnoscTreneras.AnyAsync(x =>
                x.DostepnoscId != dostepnoscId &&
                x.TrenerId == dto.TrenerId &&
                x.StartAt < dto.EndAt &&
                dto.StartAt < x.EndAt);

            if (hasConflict)
                throw new InvalidOperationException("Podany przedział czasu nakłada się na istniejącą dostępność trenera.");

            entity.TrenerId = dto.TrenerId;
            entity.KlubId = dto.KlubId;
            entity.StartAt = dto.StartAt;
            entity.EndAt = dto.EndAt;
            entity.Status = dto.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int dostepnoscId)
        {
            var entity = await _context.DostepnoscTreneras
                .FirstOrDefaultAsync(x => x.DostepnoscId == dostepnoscId);

            if (entity == null)
                return false;

            _context.DostepnoscTreneras.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static void Validate(int trenerId, DateTime startAt, DateTime endAt)
        {
            if (trenerId <= 0)
                throw new ArgumentException("TrenerId jest wymagane.");

            if (endAt <= startAt)
                throw new ArgumentException("EndAt musi być późniejsze niż StartAt.");
        }
    }
}