using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.HarmonogramZajec;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;


namespace ActiveClub.API.Services
{
    public class HarmonogramZajecService : IHarmonogramZajecService
    {
        private readonly ActiveClubContext _context;

        public HarmonogramZajecService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<HarmonogramZajecDto>> GetAllAsync()
        {
            try
            {
                return await _context.HarmonogramZajecs
                    .AsNoTracking()
                    .OrderBy(x => x.StartAt)
                    .Select(x => new HarmonogramZajecDto
                    {
                        HarmonogramId = x.HarmonogramId,
                        KlubId = x.KlubId,
                        RodzajZajecId = x.RodzajZajecId,
                        SalaId = x.SalaId,
                        TrenerId = x.TrenerId,
                        StartAt = x.StartAt,
                        EndAt = x.EndAt,
                        LimitMiejsc = x.LimitMiejsc,
                        Status = x.Status,
                        CzyWidoczneOnline = x.CzyWidoczneOnline,
                        DataUtworzenia = x.DataUtworzenia,
                        UtworzylUzytkownikId = x.UtworzylUzytkownikId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<HarmonogramZajecDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.HarmonogramZajecs
                    .AsNoTracking()
                    .Where(x => x.HarmonogramId == id)
                    .Select(x => new HarmonogramZajecDto
                    {
                        HarmonogramId = x.HarmonogramId,
                        KlubId = x.KlubId,
                        RodzajZajecId = x.RodzajZajecId,
                        SalaId = x.SalaId,
                        TrenerId = x.TrenerId,
                        StartAt = x.StartAt,
                        EndAt = x.EndAt,
                        LimitMiejsc = x.LimitMiejsc,
                        Status = x.Status,
                        CzyWidoczneOnline = x.CzyWidoczneOnline,
                        DataUtworzenia = x.DataUtworzenia,
                        UtworzylUzytkownikId = x.UtworzylUzytkownikId
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateHarmonogramZajecDto dto)
        {
            try
            {
                var entity = new HarmonogramZajec
                {
                    KlubId = dto.KlubId,
                    RodzajZajecId = dto.RodzajZajecId,
                    SalaId = dto.SalaId,
                    TrenerId = dto.TrenerId,
                    StartAt = dto.StartAt,
                    EndAt = dto.EndAt,
                    LimitMiejsc = dto.LimitMiejsc,
                    Status = dto.Status?.Trim() ?? string.Empty,
                    CzyWidoczneOnline = dto.CzyWidoczneOnline,
                    DataUtworzenia = dto.DataUtworzenia,
                    UtworzylUzytkownikId = dto.UtworzylUzytkownikId
                };

                _context.HarmonogramZajecs.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateHarmonogramZajecDto dto)
        {
            try
            {
                var entity = await _context.HarmonogramZajecs
                    .FirstOrDefaultAsync(x => x.HarmonogramId == id);

                if (entity == null)
                    return false;

                entity.KlubId = dto.KlubId;
                entity.RodzajZajecId = dto.RodzajZajecId;
                entity.SalaId = dto.SalaId;
                entity.TrenerId = dto.TrenerId;
                entity.StartAt = dto.StartAt;
                entity.EndAt = dto.EndAt;
                entity.LimitMiejsc = dto.LimitMiejsc;
                entity.Status = dto.Status?.Trim() ?? string.Empty;
                entity.CzyWidoczneOnline = dto.CzyWidoczneOnline;
                entity.DataUtworzenia = dto.DataUtworzenia;
                entity.UtworzylUzytkownikId = dto.UtworzylUzytkownikId;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<List<ActiveClub.Shared.DTOs.Harmonogram.HarmonogramZajecListDto>> GetPublicznyGrafikAsync(
    int? klubId,
    DateTime dzien)
        {
            var start = dzien.Date;
            var end = start.AddDays(1);

            var query = _context.HarmonogramZajecs
                .AsNoTracking()
                .Include(x => x.Klub)
                .Include(x => x.RodzajZajec)
                .Include(x => x.Sala)
                .Include(x => x.Trener)
                    .ThenInclude(t => t.Uzytkownik)
                .Where(x =>
                    x.CzyWidoczneOnline == true &&
                    x.StartAt >= start &&
                    x.StartAt < end);

            if (klubId.HasValue)
                query = query.Where(x => x.KlubId == klubId.Value);

            return await query
                .OrderBy(x => x.StartAt)
                .Select(x => new ActiveClub.Shared.DTOs.Harmonogram.HarmonogramZajecListDto
                {
                    HarmonogramId = x.HarmonogramId,
                    KlubId = x.KlubId,
                    KlubNazwa = x.Klub != null ? x.Klub.Nazwa : string.Empty,

                    RodzajZajecId = x.RodzajZajecId,
                    RodzajZajecNazwa = x.RodzajZajec != null ? x.RodzajZajec.Nazwa : string.Empty,
                    RodzajZajecOpis = x.RodzajZajec != null ? x.RodzajZajec.Opis : string.Empty,

                    SalaId = x.SalaId,

                    TrenerId = x.TrenerId,
                    TrenerImie = x.Trener != null && x.Trener.Uzytkownik != null
                        ? x.Trener.Uzytkownik.Imie
                        : string.Empty,
                    TrenerNazwisko = x.Trener != null && x.Trener.Uzytkownik != null
                        ? x.Trener.Uzytkownik.Nazwisko
                        : string.Empty,

                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    LimitMiejsc = x.LimitMiejsc,
                    Status = x.Status,
                    CzyWidoczneOnline = x.CzyWidoczneOnline,
                    LiczbaZapisanych = x.RejestracjeNaZajecia
    .Count(r => r.Status == "ZAPISANY"),
                    LiczbaWolnychMiejsc =
    (x.LimitMiejsc ?? 0)
    - x.RejestracjeNaZajecia.Count(r => r.Status == "ZAPISANY"),
                })
                .ToListAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.HarmonogramZajecs
                    .FirstOrDefaultAsync(x => x.HarmonogramId == id);

                if (entity == null)
                    return false;

                _context.HarmonogramZajecs.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync HarmonogramZajec:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}