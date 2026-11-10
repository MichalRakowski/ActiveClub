using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services;

public class RodzajeCzlonkostwaService : IRodzajeCzlonkostwaService
{
    private readonly ActiveClubContext _context;

    public RodzajeCzlonkostwaService(ActiveClubContext context)
    {
        _context = context;
    }

    public async Task<List<RodzajCzlonkostwaDto>> GetAllAsync()
    {
        return await _context.RodzajeCzlonkostwas
            .AsNoTracking()
            .OrderBy(x => x.Nazwa)
            .Select(x => new RodzajCzlonkostwaDto
            {
                RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                Kod = x.Kod,
                Nazwa = x.Nazwa,
                Opis = x.Opis,
                Cena = x.Cena,
                TypRozliczenia = x.TypRozliczenia,
                OkresDni = x.OkresDni,
                LiczbaWejsc = x.LiczbaWejsc,
                CzyAktywny = x.CzyAktywny,
                CzyWidoczneOnline = x.CzyWidoczneOnline,
                DataUtworzenia = x.DataUtworzenia
            })
            .ToListAsync();
    }

    public async Task<RodzajCzlonkostwaDto?> GetByIdAsync(int id)
    {
        return await _context.RodzajeCzlonkostwas
            .AsNoTracking()
            .Where(x => x.RodzajCzlonkostwaId == id)
            .Select(x => new RodzajCzlonkostwaDto
            {
                RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                Kod = x.Kod,
                Nazwa = x.Nazwa,
                Opis = x.Opis,
                Cena = x.Cena,
                TypRozliczenia = x.TypRozliczenia,
                OkresDni = x.OkresDni,
                LiczbaWejsc = x.LiczbaWejsc,
                CzyAktywny = x.CzyAktywny,
                CzyWidoczneOnline = x.CzyWidoczneOnline,
                DataUtworzenia = x.DataUtworzenia
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByNameAsync(string nazwa, int? excludeId = null)
    {
        nazwa = nazwa.Trim();

        return await _context.RodzajeCzlonkostwas
            .AnyAsync(x =>
                x.Nazwa == nazwa &&
                (!excludeId.HasValue || x.RodzajCzlonkostwaId != excludeId.Value));
    }

    public async Task<bool> ExistsByCodeAsync(string kod, int? excludeId = null)
    {
        kod = kod.Trim();

        return await _context.RodzajeCzlonkostwas
            .AnyAsync(x =>
                x.Kod == kod &&
                (!excludeId.HasValue || x.RodzajCzlonkostwaId != excludeId.Value));
    }

    public async Task<RodzajCzlonkostwaDto> CreateAsync(CreateRodzajCzlonkostwaDto dto)
    {
        var item = new RodzajeCzlonkostwa
        {
            Kod = dto.Kod.Trim(),
            Nazwa = dto.Nazwa.Trim(),
            Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim(),
            Cena = dto.Cena,
            TypRozliczenia = dto.TypRozliczenia.Trim(),
            OkresDni = dto.OkresDni,
            LiczbaWejsc = dto.LiczbaWejsc,
            CzyAktywny = dto.CzyAktywny,
            CzyWidoczneOnline = dto.CzyWidoczneOnline,
            DataUtworzenia = DateTime.UtcNow
        };

        NormalizeFieldsByTypRozliczenia(item);

        _context.RodzajeCzlonkostwas.Add(item);
        await _context.SaveChangesAsync();

        return new RodzajCzlonkostwaDto
        {
            RodzajCzlonkostwaId = item.RodzajCzlonkostwaId,
            Kod = item.Kod,
            Nazwa = item.Nazwa,
            Opis = item.Opis,
            Cena = item.Cena,
            TypRozliczenia = item.TypRozliczenia,
            OkresDni = item.OkresDni,
            LiczbaWejsc = item.LiczbaWejsc,
            CzyAktywny = item.CzyAktywny,
            CzyWidoczneOnline = item.CzyWidoczneOnline,
            DataUtworzenia = item.DataUtworzenia
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateRodzajCzlonkostwaDto dto)
    {
        var entity = await _context.RodzajeCzlonkostwas
            .FirstOrDefaultAsync(x => x.RodzajCzlonkostwaId == id);

        if (entity == null)
            return false;

        entity.Kod = dto.Kod.Trim();
        entity.Nazwa = dto.Nazwa.Trim();
        entity.Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim();
        entity.Cena = dto.Cena;
        entity.TypRozliczenia = dto.TypRozliczenia.Trim();
        entity.OkresDni = dto.OkresDni;
        entity.LiczbaWejsc = dto.LiczbaWejsc;
        entity.CzyAktywny = dto.CzyAktywny;
        entity.CzyWidoczneOnline = dto.CzyWidoczneOnline;

        NormalizeFieldsByTypRozliczenia(entity);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.RodzajeCzlonkostwas
            .FirstOrDefaultAsync(x => x.RodzajCzlonkostwaId == id);

        if (entity == null)
            return false;

        _context.RodzajeCzlonkostwas.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    private static void NormalizeFieldsByTypRozliczenia(RodzajeCzlonkostwa entity)
    {
        switch (entity.TypRozliczenia)
        {
            case "Okresowe":
                entity.LiczbaWejsc = null;
                break;

            case "NaWejścia":
                entity.OkresDni = null;
                break;

            case "Jednorazowe":
                entity.OkresDni = null;
                entity.LiczbaWejsc = null;
                break;
        }
    }
}