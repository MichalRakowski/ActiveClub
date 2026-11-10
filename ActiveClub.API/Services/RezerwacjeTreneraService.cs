using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.RezerwacjeTrenera;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class RezerwacjeTreneraService : IRezerwacjeTreneraService
    {
        private readonly ActiveClubContext _context;

        public RezerwacjeTreneraService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RezerwacjaTreneraDto>> GetAllAsync()
        {
            return await _context.RezerwacjeTreneras
                .Include(x => x.Trener)
                .Include(x => x.Klub)
                .OrderByDescending(x => x.StartAt)
                .Select(x => new RezerwacjaTreneraDto
                {
                    RezerwacjaId = x.RezerwacjaId,
                    TrenerId = x.TrenerId,
                    UzytkownikId = x.UzytkownikId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    Cena = x.Cena,
                    DataUtworzenia = x.DataUtworzenia,

                    TrenerNazwa = x.Trener != null
                        ? (x.Trener.Imie + " " + x.Trener.Nazwisko)
                        : string.Empty,

                    KlubNazwa = x.Klub != null
                        ? x.Klub.Nazwa
                        : string.Empty
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RezerwacjaTreneraDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            return await _context.RezerwacjeTreneras
                .Include(x => x.Trener)
                .Include(x => x.Klub)
                .Where(x => x.UzytkownikId == uzytkownikId)
                .OrderByDescending(x => x.StartAt)
                .Select(x => new RezerwacjaTreneraDto
                {
                    RezerwacjaId = x.RezerwacjaId,
                    TrenerId = x.TrenerId,
                    UzytkownikId = x.UzytkownikId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    Cena = x.Cena,
                    DataUtworzenia = x.DataUtworzenia,

                    TrenerNazwa = x.Trener != null
                        ? (x.Trener.Imie + " " + x.Trener.Nazwisko)
                        : string.Empty,

                    KlubNazwa = x.Klub != null
                        ? x.Klub.Nazwa
                        : string.Empty
                })
                .ToListAsync();
        }

        public async Task<RezerwacjaTreneraDto?> GetByIdAsync(int rezerwacjaId)
        {
            return await _context.RezerwacjeTreneras
                .Include(x => x.Trener)
                .Include(x => x.Klub)
                .Where(x => x.RezerwacjaId == rezerwacjaId)
                .Select(x => new RezerwacjaTreneraDto
                {
                    RezerwacjaId = x.RezerwacjaId,
                    TrenerId = x.TrenerId,
                    UzytkownikId = x.UzytkownikId,
                    KlubId = x.KlubId,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    Status = x.Status,
                    Cena = x.Cena,
                    DataUtworzenia = x.DataUtworzenia,

                    TrenerNazwa = x.Trener != null
                        ? (x.Trener.Imie + " " + x.Trener.Nazwisko)
                        : string.Empty,

                    KlubNazwa = x.Klub != null
                        ? x.Klub.Nazwa
                        : string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<RezerwacjaTreneraDto> CreateAsync(CreateRezerwacjaTreneraDto dto)
        {
            var trener = await _context.Trenerzies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TrenerId == dto.TrenerId);

            var entity = new RezerwacjeTrenera
            {
                TrenerId = dto.TrenerId,
                UzytkownikId = dto.UzytkownikId,
                KlubId = dto.KlubId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status,
                Cena = dto.Cena ?? trener?.StawkaZaGodzine,
                DataUtworzenia = DateTime.Now
            };

            _context.RezerwacjeTreneras.Add(entity);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(entity.RezerwacjaId)
                   ?? new RezerwacjaTreneraDto
                   {
                       RezerwacjaId = entity.RezerwacjaId,
                       TrenerId = entity.TrenerId,
                       UzytkownikId = entity.UzytkownikId,
                       KlubId = entity.KlubId,
                       StartAt = entity.StartAt,
                       EndAt = entity.EndAt,
                       Status = entity.Status,
                       Cena = entity.Cena,
                       DataUtworzenia = entity.DataUtworzenia
                   };
        }

        public async Task<bool> UpdateAsync(int rezerwacjaId, UpdateRezerwacjaTreneraDto dto)
        {
            var entity = await _context.RezerwacjeTreneras
                .FirstOrDefaultAsync(x => x.RezerwacjaId == rezerwacjaId);

            if (entity == null)
                return false;

            entity.TrenerId = dto.TrenerId;
            entity.UzytkownikId = dto.UzytkownikId;
            entity.KlubId = dto.KlubId;
            entity.StartAt = dto.StartAt;
            entity.EndAt = dto.EndAt;
            entity.Status = dto.Status;
            entity.Cena = dto.Cena;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int rezerwacjaId)
        {
            var entity = await _context.RezerwacjeTreneras
                .FirstOrDefaultAsync(x => x.RezerwacjaId == rezerwacjaId);

            if (entity == null)
                return false;

            _context.RezerwacjeTreneras.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}