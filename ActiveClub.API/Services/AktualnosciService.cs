using ActiveClub.Shared.DTOs.Aktualnosci;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services;

public class AktualnosciService : IAktualnosciService
{
    private readonly ActiveClubContext _context;

    public AktualnosciService(ActiveClubContext context)
    {
        _context = context;
    }

    public async Task<List<AktualnoscDto>> GetAllAsync()
    {
        return await _context.Aktualnoscis
            .AsNoTracking()
            .OrderByDescending(x => x.DataPublikacji ?? x.DataDodania)
            .Select(x => new AktualnoscDto
            {
                AktualnoscId = x.AktualnoscId,
                Tytul = x.Tytul,
                Tresc = x.Tresc,
                KlubId = x.KlubId,
                CzyWidoczna = x.CzyWidoczna,
                DataPublikacji = x.DataPublikacji,
                DataDodania = x.DataDodania,
                DodanePrzezUzytkownikId = x.DodanePrzezUzytkownikId
            })
            .ToListAsync();
    }

    public async Task<AktualnoscDto?> GetByIdAsync(int id)
    {
        return await _context.Aktualnoscis
            .AsNoTracking()
            .Where(x => x.AktualnoscId == id)
            .Select(x => new AktualnoscDto
            {
                AktualnoscId = x.AktualnoscId,
                Tytul = x.Tytul,
                Tresc = x.Tresc,
                KlubId = x.KlubId,
                CzyWidoczna = x.CzyWidoczna,
                DataPublikacji = x.DataPublikacji,
                DataDodania = x.DataDodania,
                DodanePrzezUzytkownikId = x.DodanePrzezUzytkownikId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AktualnoscDto> CreateAsync(CreateAktualnoscDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Tytul))
            throw new ArgumentException("Tytuł jest wymagany.");

        var entity = new Aktualnosci
        {
            Tytul = dto.Tytul,
            Tresc = dto.Tresc,
            KlubId = dto.KlubId,
            CzyWidoczna = dto.CzyWidoczna ?? true,
            DataPublikacji = dto.DataPublikacji,
            DataDodania = dto.DataDodania,
            DodanePrzezUzytkownikId = dto.DodanePrzezUzytkownikId
        };

        _context.Aktualnoscis.Add(entity);
        await _context.SaveChangesAsync();

        return new AktualnoscDto
        {
            AktualnoscId = entity.AktualnoscId,
            Tytul = entity.Tytul,
            Tresc = entity.Tresc,
            KlubId = entity.KlubId,
            CzyWidoczna = entity.CzyWidoczna,
            DataPublikacji = entity.DataPublikacji,
            DataDodania = entity.DataDodania,
            DodanePrzezUzytkownikId = entity.DodanePrzezUzytkownikId
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateAktualnoscDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Tytul))
            throw new ArgumentException("Tytuł jest wymagany.");

        var entity = await _context.Aktualnoscis
            .FirstOrDefaultAsync(x => x.AktualnoscId == id);

        if (entity == null)
            return false;

        entity.Tytul = dto.Tytul;
        entity.Tresc = dto.Tresc;
        entity.KlubId = dto.KlubId;
        entity.CzyWidoczna = dto.CzyWidoczna;
        entity.DataPublikacji = dto.DataPublikacji;
        entity.DodanePrzezUzytkownikId = dto.DodanePrzezUzytkownikId;

        if (entity.DataDodania == null && dto.DataDodania != null)
            entity.DataDodania = dto.DataDodania;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Aktualnoscis
            .FirstOrDefaultAsync(x => x.AktualnoscId == id);

        if (entity == null)
            return false;

        _context.Aktualnoscis.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }
}