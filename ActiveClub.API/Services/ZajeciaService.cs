using ActiveClub.Shared.DTOs.Zajecia;
using ActiveClub.API.Services.Interfaces;
using ActiveClubData.Data;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services;

public class ZajeciaService : IZajeciaService
{
    private readonly ActiveClubContext _context;

    public ZajeciaService(ActiveClubContext context)
    {
        _context = context;
    }

    public async Task<List<ZajecieDto>> GetZajeciaAsync()
    {
        return await _context.RodzajeZajecs
            .AsNoTracking()
            .OrderBy(z => z.Nazwa)
            .Select(z => new ZajecieDto
            {
                RodzajZajecId = z.RodzajZajecId,
                Nazwa = z.Nazwa,
                Opis = z.Opis
            })
            .ToListAsync();
    }
}