using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.OfertyKlubuCzlonkostwa;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class OfertyKlubuCzlonkostwaService : IOfertyKlubuCzlonkostwaService
    {
        private readonly ActiveClubContext _context;

        public OfertyKlubuCzlonkostwaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<OfertaKlubuCzlonkostwoDto>> GetAllAsync()
        {
            try
            {
                return await _context.OfertaKlubuCzlonkostwos
                    .AsNoTracking()
                    .Include(x => x.Klub)
                    .Include(x => x.RodzajCzlonkostwa)
                    .OrderBy(x => x.Kolejnosc)
                    .ThenBy(x => x.OfertaKlubuCzlonkostwoId)
                    .Select(x => new OfertaKlubuCzlonkostwoDto
                    {
                        OfertaKlubuCzlonkostwoId = x.OfertaKlubuCzlonkostwoId,
                        KlubId = x.KlubId,
                        KlubNazwa = x.Klub.Nazwa,
                        RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                        RodzajCzlonkostwaNazwa = x.RodzajCzlonkostwa.Nazwa,
                        Cena = x.Cena,
                        Waluta = x.Waluta,
                        CzyAktywna = x.CzyAktywna,
                        CzyWidoczneOnline = x.CzyWidoczneOnline,
                        DataOd = x.DataOd.HasValue
                            ? x.DataOd.Value.ToDateTime(TimeOnly.MinValue)
                            : null,
                        DataDo = x.DataDo.HasValue
                            ? x.DataDo.Value.ToDateTime(TimeOnly.MinValue)
                            : null,
                        Kolejnosc = x.Kolejnosc,
                        OpisMarketingowy = x.OpisMarketingowy,
                        DataUtworzenia = x.DataUtworzenia
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<OfertaKlubuCzlonkostwoDto?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.OfertaKlubuCzlonkostwos
                    .AsNoTracking()
                    .Include(x => x.Klub)
                    .Include(x => x.RodzajCzlonkostwa)
                    .Where(x => x.OfertaKlubuCzlonkostwoId == id)
                    .Select(x => new OfertaKlubuCzlonkostwoDto
                    {
                        OfertaKlubuCzlonkostwoId = x.OfertaKlubuCzlonkostwoId,
                        KlubId = x.KlubId,
                        KlubNazwa = x.Klub.Nazwa,
                        RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                        RodzajCzlonkostwaNazwa = x.RodzajCzlonkostwa.Nazwa,
                        Cena = x.Cena,
                        Waluta = x.Waluta,
                        CzyAktywna = x.CzyAktywna,
                        CzyWidoczneOnline = x.CzyWidoczneOnline,
                        DataOd = x.DataOd.HasValue
                            ? x.DataOd.Value.ToDateTime(TimeOnly.MinValue)
                            : null,
                        DataDo = x.DataDo.HasValue
                            ? x.DataDo.Value.ToDateTime(TimeOnly.MinValue)
                            : null,
                        Kolejnosc = x.Kolejnosc,
                        OpisMarketingowy = x.OpisMarketingowy,
                        DataUtworzenia = x.DataUtworzenia
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<OfertaKlubuCzlonkostwoFormDataDto> GetFormDataAsync()
        {
            try
            {
                var dto = new OfertaKlubuCzlonkostwoFormDataDto
                {
                    Kluby = await _context.Klubies
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.KlubId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync(),

                    RodzajeCzlonkostwa = await _context.RodzajeCzlonkostwas
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.RodzajCzlonkostwaId,
                            Nazwa = x.Nazwa
                        })
                        .ToListAsync()
                };

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateOfertaKlubuCzlonkostwoDto dto)
        {
            try
            {
                var entity = new OfertaKlubuCzlonkostwo
                {
                    KlubId = dto.KlubId,
                    RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId,
                    Cena = dto.Cena,
                    Waluta = dto.Waluta?.Trim() ?? string.Empty,
                    CzyAktywna = dto.CzyAktywna,
                    CzyWidoczneOnline = dto.CzyWidoczneOnline,
                    DataOd = dto.DataOd.HasValue ? DateOnly.FromDateTime(dto.DataOd.Value) : null,
                    DataDo = dto.DataDo.HasValue ? DateOnly.FromDateTime(dto.DataDo.Value) : null,
                    Kolejnosc = dto.Kolejnosc,
                    OpisMarketingowy = dto.OpisMarketingowy,
                    DataUtworzenia = DateTime.Now
                };

                _context.OfertaKlubuCzlonkostwos.Add(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE CreateAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateOfertaKlubuCzlonkostwoDto dto)
        {
            try
            {
                var entity = await _context.OfertaKlubuCzlonkostwos
                    .FirstOrDefaultAsync(x => x.OfertaKlubuCzlonkostwoId == id);

                if (entity == null)
                    return false;

                entity.KlubId = dto.KlubId;
                entity.RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId;
                entity.Cena = dto.Cena;
                entity.Waluta = dto.Waluta?.Trim() ?? string.Empty;
                entity.CzyAktywna = dto.CzyAktywna;
                entity.CzyWidoczneOnline = dto.CzyWidoczneOnline;
                entity.DataOd = dto.DataOd.HasValue ? DateOnly.FromDateTime(dto.DataOd.Value) : null;
                entity.DataDo = dto.DataDo.HasValue ? DateOnly.FromDateTime(dto.DataDo.Value) : null;
                entity.Kolejnosc = dto.Kolejnosc;
                entity.OpisMarketingowy = dto.OpisMarketingowy;

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.OfertaKlubuCzlonkostwos
                    .FirstOrDefaultAsync(x => x.OfertaKlubuCzlonkostwoId == id);

                if (entity == null)
                    return false;

                _context.OfertaKlubuCzlonkostwos.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync OfertyKlubuCzlonkostwa:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}