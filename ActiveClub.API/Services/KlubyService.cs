using ActiveClubData.Data;
using ActiveClubData.Models;
using ActiveClub.Shared.DTOs.Kluby;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services;

public class KlubyService : IKlubyService
{
    private readonly ActiveClubContext _context;

    public KlubyService(ActiveClubContext context)
    {
        _context = context;
    }

    public async Task<List<KlubDto>> GetAsync(string? q, string? miasto)
    {
        IQueryable<Kluby> query = _context.Klubies
            .AsNoTracking()
            .Where(k => k.CzyAktywny);

        if (!string.IsNullOrWhiteSpace(miasto))
        {
            miasto = miasto.Trim();
            query = query.Where(k => k.Miasto != null && k.Miasto == miasto);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();

            query = query.Where(k =>
                k.Nazwa.Contains(q) ||
                (k.Miasto != null && k.Miasto.Contains(q)) ||
                (k.Ulica != null && k.Ulica.Contains(q)) ||
                (k.KodPocztowy != null && k.KodPocztowy.Contains(q)) ||
                (k.Telefon != null && k.Telefon.Contains(q)) ||
                (k.EmailKontakt != null && k.EmailKontakt.Contains(q)) ||
                (k.Opis != null && k.Opis.Contains(q)));
        }

        return await query
            .OrderBy(k => k.Miasto)
            .ThenBy(k => k.Nazwa)
            .Select(k => new KlubDto
            {
                KlubId = k.KlubId,
                Nazwa = k.Nazwa,
                Miasto = k.Miasto,
                Ulica = k.Ulica,
                KodPocztowy = k.KodPocztowy,
                Telefon = k.Telefon,
                EmailKontakt = k.EmailKontakt,
                Opis = k.Opis,
                CzyAktywny = k.CzyAktywny,
                DataUtworzenia = k.DataUtworzenia,
                DataModyfikacji = k.DataModyfikacji
            })
            .ToListAsync();
    }

    public async Task<List<string>> GetMiastaAsync()
    {
        return await _context.Klubies
            .AsNoTracking()
            .Where(k => k.CzyAktywny && !string.IsNullOrWhiteSpace(k.Miasto))
            .Select(k => k.Miasto!)
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync();
    }

    public async Task<KlubDto?> GetByIdAsync(int id)
    {
        return await _context.Klubies
            .AsNoTracking()
            .Where(k => k.KlubId == id && k.CzyAktywny)
            .Select(k => new KlubDto
            {
                KlubId = k.KlubId,
                Nazwa = k.Nazwa,
                Miasto = k.Miasto,
                Ulica = k.Ulica,
                KodPocztowy = k.KodPocztowy,
                Telefon = k.Telefon,
                EmailKontakt = k.EmailKontakt,
                Opis = k.Opis,
                CzyAktywny = k.CzyAktywny,
                DataUtworzenia = k.DataUtworzenia,
                DataModyfikacji = k.DataModyfikacji
            })
            .FirstOrDefaultAsync();
    }

    public async Task<KlubDto> CreateAsync(CreateKlubDto dto)
    {
        var klub = new Kluby
        {
            Nazwa = dto.Nazwa.Trim(),
            Miasto = string.IsNullOrWhiteSpace(dto.Miasto) ? null : dto.Miasto.Trim(),
            Ulica = string.IsNullOrWhiteSpace(dto.Ulica) ? null : dto.Ulica.Trim(),
            KodPocztowy = string.IsNullOrWhiteSpace(dto.KodPocztowy) ? null : dto.KodPocztowy.Trim(),
            Telefon = string.IsNullOrWhiteSpace(dto.Telefon) ? null : dto.Telefon.Trim(),
            EmailKontakt = string.IsNullOrWhiteSpace(dto.EmailKontakt) ? null : dto.EmailKontakt.Trim(),
            Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim(),
            CzyAktywny = dto.CzyAktywny,
            DataUtworzenia = dto.DataUtworzenia == default ? DateTime.Now : dto.DataUtworzenia,
            DataModyfikacji = null
        };

        _context.Klubies.Add(klub);
        await _context.SaveChangesAsync();

        return new KlubDto
        {
            KlubId = klub.KlubId,
            Nazwa = klub.Nazwa,
            Miasto = klub.Miasto,
            Ulica = klub.Ulica,
            KodPocztowy = klub.KodPocztowy,
            Telefon = klub.Telefon,
            EmailKontakt = klub.EmailKontakt,
            Opis = klub.Opis,
            CzyAktywny = klub.CzyAktywny,
            DataUtworzenia = klub.DataUtworzenia,
            DataModyfikacji = klub.DataModyfikacji
        };
    }

    public async Task<KlubDto?> UpdateAsync(UpdateKlubDto dto)
    {
        var klub = await _context.Klubies.FirstOrDefaultAsync(k => k.KlubId == dto.KlubId);

        if (klub == null)
            return null;

        klub.Nazwa = dto.Nazwa.Trim();
        klub.Miasto = string.IsNullOrWhiteSpace(dto.Miasto) ? null : dto.Miasto.Trim();
        klub.Ulica = string.IsNullOrWhiteSpace(dto.Ulica) ? null : dto.Ulica.Trim();
        klub.KodPocztowy = string.IsNullOrWhiteSpace(dto.KodPocztowy) ? null : dto.KodPocztowy.Trim();
        klub.Telefon = string.IsNullOrWhiteSpace(dto.Telefon) ? null : dto.Telefon.Trim();
        klub.EmailKontakt = string.IsNullOrWhiteSpace(dto.EmailKontakt) ? null : dto.EmailKontakt.Trim();
        klub.Opis = string.IsNullOrWhiteSpace(dto.Opis) ? null : dto.Opis.Trim();
        klub.CzyAktywny = dto.CzyAktywny;
        klub.DataUtworzenia = dto.DataUtworzenia;
        klub.DataModyfikacji = DateTime.Now;

        await _context.SaveChangesAsync();

        return new KlubDto
        {
            KlubId = klub.KlubId,
            Nazwa = klub.Nazwa,
            Miasto = klub.Miasto,
            Ulica = klub.Ulica,
            KodPocztowy = klub.KodPocztowy,
            Telefon = klub.Telefon,
            EmailKontakt = klub.EmailKontakt,
            Opis = klub.Opis,
            CzyAktywny = klub.CzyAktywny,
            DataUtworzenia = klub.DataUtworzenia,
            DataModyfikacji = klub.DataModyfikacji
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var klub = await _context.Klubies.FirstOrDefaultAsync(k => k.KlubId == id);

        if (klub == null)
            return false;

        _context.Klubies.Remove(klub);
        await _context.SaveChangesAsync();

        return true;
    }
}