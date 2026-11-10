using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class CzlonkostwaUzytkownikaService : ICzlonkostwaUzytkownikaService
    {
        private readonly ActiveClubContext _context;

        public CzlonkostwaUzytkownikaService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<List<CzlonkostwoUzytkownikaDto>> GetAllAsync()
        {
            try
            {
                var users = await _context.Set<ApplicationUser>()
                    .AsNoTracking()
                    .Select(u => new
                    {
                        u.Id,
                        PelnaNazwa = ((u.Imie ?? "") + " " + (u.Nazwisko ?? "")).Trim()
                    })
                    .ToDictionaryAsync(x => x.Id, x => x.PelnaNazwa);

                var items = await _context.CzlonkostwaUzytkownikas
                    .AsNoTracking()
                    .Include(x => x.Uzytkownik)
                    .Include(x => x.OfertaKlubuCzlonkostwo)
                        .ThenInclude(x => x.Klub)
                    .Include(x => x.OfertaKlubuCzlonkostwo)
                        .ThenInclude(x => x.RodzajCzlonkostwa)
                    .Include(x => x.RodzajCzlonkostwa)
                    .Include(x => x.Klub)
                    .Include(x => x.Zamowienie)
                    .OrderByDescending(x => x.CzlonkostwoId)
                    .ToListAsync();

                return items.Select(x => new CzlonkostwoUzytkownikaDto
                {
                    CzlonkostwoId = x.CzlonkostwoId,

                    UzytkownikId = x.UzytkownikId,
                    UzytkownikNazwa = x.Uzytkownik != null
                        ? ((x.Uzytkownik.Imie ?? "") + " " + (x.Uzytkownik.Nazwisko ?? "")).Trim()
                        : string.Empty,

                    OfertaKlubuCzlonkostwoId = x.OfertaKlubuCzlonkostwoId,
                    OfertaKlubuCzlonkostwoNazwa = x.OfertaKlubuCzlonkostwo != null
                        ? (
                            (x.OfertaKlubuCzlonkostwo.Klub != null ? x.OfertaKlubuCzlonkostwo.Klub.Nazwa : "Brak klubu")
                            + " - " +
                            (x.OfertaKlubuCzlonkostwo.RodzajCzlonkostwa != null ? x.OfertaKlubuCzlonkostwo.RodzajCzlonkostwa.Nazwa : "Brak rodzaju")
                            + " - " +
                            x.OfertaKlubuCzlonkostwo.Cena.ToString("0.##")
                            + " " +
                            x.OfertaKlubuCzlonkostwo.Waluta
                          )
                        : string.Empty,

                    RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                    RodzajCzlonkostwaNazwa = x.RodzajCzlonkostwa != null
                        ? (x.RodzajCzlonkostwa.Nazwa ?? $"Rodzaj #{x.RodzajCzlonkostwaId}")
                        : string.Empty,

                    KlubId = x.KlubId,
                    KlubNazwa = x.Klub != null
                        ? (x.Klub.Nazwa ?? $"Klub #{x.KlubId}")
                        : string.Empty,

                    StartAt = x.StartAt.HasValue ? x.StartAt.Value.ToDateTime(TimeOnly.MinValue) : null,
                    EndAt = x.EndAt.HasValue ? x.EndAt.Value.ToDateTime(TimeOnly.MinValue) : null,

                    Status = x.Status,
                    CenaZakupu = x.CenaZakupu,

                    ZamowienieId = x.ZamowienieId,
                    ZamowienieNazwa = x.Zamowienie != null
                        ? $"Zamówienie #{x.Zamowienie.ZamowienieId}"
                        : string.Empty,

                    NadanePrzezUzytkownikId = x.NadanePrzezUzytkownikId,
                    NadanePrzezUzytkownikNazwa = x.NadanePrzezUzytkownikId.HasValue &&
                                                 users.TryGetValue(x.NadanePrzezUzytkownikId.Value, out var nadanePrzez)
                        ? nadanePrzez
                        : string.Empty,

                    DataUtworzenia = x.DataUtworzenia,
                    DataAktywacji = x.DataAktywacji,
                    DataAnulowania = x.DataAnulowania,
                    PowodAnulowania = x.PowodAnulowania,
                    LiczbaWejscLimit = x.LiczbaWejscLimit,
                    LiczbaWejscWykorzystanych = x.LiczbaWejscWykorzystanych
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetAllAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<CzlonkostwoUzytkownikaDto?> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.CzlonkostwaUzytkownikas
                    .AsNoTracking()
                    .Include(x => x.Uzytkownik)
                    .Include(x => x.OfertaKlubuCzlonkostwo)
                        .ThenInclude(x => x.Klub)
                    .Include(x => x.OfertaKlubuCzlonkostwo)
                        .ThenInclude(x => x.RodzajCzlonkostwa)
                    .Include(x => x.RodzajCzlonkostwa)
                    .Include(x => x.Klub)
                    .Include(x => x.Zamowienie)
                    .FirstOrDefaultAsync(x => x.CzlonkostwoId == id);

                if (entity == null)
                    return null;

                string nadanePrzezNazwa = string.Empty;

                if (entity.NadanePrzezUzytkownikId.HasValue)
                {
                    nadanePrzezNazwa = await _context.Set<ApplicationUser>()
                        .AsNoTracking()
                        .Where(u => u.Id == entity.NadanePrzezUzytkownikId.Value)
                        .Select(u => ((u.Imie ?? "") + " " + (u.Nazwisko ?? "")).Trim())
                        .FirstOrDefaultAsync() ?? string.Empty;
                }

                return new CzlonkostwoUzytkownikaDto
                {
                    CzlonkostwoId = entity.CzlonkostwoId,

                    UzytkownikId = entity.UzytkownikId,
                    UzytkownikNazwa = entity.Uzytkownik != null
                        ? ((entity.Uzytkownik.Imie ?? "") + " " + (entity.Uzytkownik.Nazwisko ?? "")).Trim()
                        : string.Empty,

                    OfertaKlubuCzlonkostwoId = entity.OfertaKlubuCzlonkostwoId,
                    OfertaKlubuCzlonkostwoNazwa = entity.OfertaKlubuCzlonkostwo != null
                        ? (
                            (entity.OfertaKlubuCzlonkostwo.Klub != null ? entity.OfertaKlubuCzlonkostwo.Klub.Nazwa : "Brak klubu")
                            + " - " +
                            (entity.OfertaKlubuCzlonkostwo.RodzajCzlonkostwa != null ? entity.OfertaKlubuCzlonkostwo.RodzajCzlonkostwa.Nazwa : "Brak rodzaju")
                            + " - " +
                            entity.OfertaKlubuCzlonkostwo.Cena.ToString("0.##")
                            + " " +
                            entity.OfertaKlubuCzlonkostwo.Waluta
                          )
                        : string.Empty,

                    RodzajCzlonkostwaId = entity.RodzajCzlonkostwaId,
                    RodzajCzlonkostwaNazwa = entity.RodzajCzlonkostwa != null
                        ? (entity.RodzajCzlonkostwa.Nazwa ?? $"Rodzaj #{entity.RodzajCzlonkostwaId}")
                        : string.Empty,

                    KlubId = entity.KlubId,
                    KlubNazwa = entity.Klub != null
                        ? (entity.Klub.Nazwa ?? $"Klub #{entity.KlubId}")
                        : string.Empty,

                    StartAt = entity.StartAt.HasValue ? entity.StartAt.Value.ToDateTime(TimeOnly.MinValue) : null,
                    EndAt = entity.EndAt.HasValue ? entity.EndAt.Value.ToDateTime(TimeOnly.MinValue) : null,

                    Status = entity.Status,
                    CenaZakupu = entity.CenaZakupu,

                    ZamowienieId = entity.ZamowienieId,
                    ZamowienieNazwa = entity.Zamowienie != null
                        ? $"Zamówienie #{entity.Zamowienie.ZamowienieId}"
                        : string.Empty,

                    NadanePrzezUzytkownikId = entity.NadanePrzezUzytkownikId,
                    NadanePrzezUzytkownikNazwa = nadanePrzezNazwa,

                    DataUtworzenia = entity.DataUtworzenia,
                    DataAktywacji = entity.DataAktywacji,
                    DataAnulowania = entity.DataAnulowania,
                    PowodAnulowania = entity.PowodAnulowania,
                    LiczbaWejscLimit = entity.LiczbaWejscLimit,
                    LiczbaWejscWykorzystanych = entity.LiczbaWejscWykorzystanych
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetByIdAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<CzlonkostwoUzytkownikaFormDataDto> GetFormDataAsync()
        {
            try
            {
                return new CzlonkostwoUzytkownikaFormDataDto
                {
                    Uzytkownicy = await _context.Set<ApplicationUser>()
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwisko)
                        .ThenBy(x => x.Imie)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.Id,
                            Nazwa = ((x.Imie ?? "") + " " + (x.Nazwisko ?? "")).Trim()
                        })
                        .ToListAsync(),

                    OfertyKlubuCzlonkostwa = await _context.OfertaKlubuCzlonkostwos
                        .AsNoTracking()
                        .Include(x => x.Klub)
                        .Include(x => x.RodzajCzlonkostwa)
                        .OrderBy(x => x.Klub.Nazwa)
                        .ThenBy(x => x.RodzajCzlonkostwa.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.OfertaKlubuCzlonkostwoId,
                            Nazwa =
                                (x.Klub != null ? x.Klub.Nazwa : "Brak klubu")
                                + " - " +
                                (x.RodzajCzlonkostwa != null ? x.RodzajCzlonkostwa.Nazwa : "Brak rodzaju")
                                + " - " +
                                x.Cena.ToString("0.##")
                                + " " +
                                x.Waluta
                        })
                        .ToListAsync(),

                    RodzajeCzlonkostwa = await _context.RodzajeCzlonkostwas
    .AsNoTracking()
    .Where(x => x.CzyAktywny)
    .OrderBy(x => x.Nazwa)
    .Select(x => new LookupItemDto
    {
        Id = x.RodzajCzlonkostwaId,
        Nazwa = x.Nazwa ?? $"Rodzaj #{x.RodzajCzlonkostwaId}"
    })
    .ToListAsync(),

                    Kluby = await _context.Klubies
                        .AsNoTracking()
                        .OrderBy(x => x.Nazwa)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.KlubId,
                            Nazwa = x.Nazwa ?? $"Klub #{x.KlubId}"
                        })
                        .ToListAsync(),

                    Zamowienia = await _context.Zamowienia
                        .AsNoTracking()
                        .OrderByDescending(x => x.ZamowienieId)
                        .Select(x => new LookupItemDto
                        {
                            Id = x.ZamowienieId,
                            Nazwa = $"Zamówienie #{x.ZamowienieId}"
                        })
                        .ToListAsync(),

                    NadajacyUzytkownicy = new List<LookupItemDto>(),

                    Statusy = new List<LookupItemDto>
                    {
                        new LookupItemDto { Id = 1, Nazwa = "Nowe" },
                        new LookupItemDto { Id = 2, Nazwa = "Aktywne" },
                        new LookupItemDto { Id = 3, Nazwa = "Anulowane" },
                        new LookupItemDto { Id = 4, Nazwa = "Wygasłe" }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetFormDataAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<LookupItemDto>> GetPracownicyKlubuAsync(int klubId)
        {
            try
            {
                return await (
                    from pk in _context.PracownicyKlubus.AsNoTracking()
                    join u in _context.Users.AsNoTracking() on pk.UzytkownikId equals u.Id
                    where pk.KlubId == klubId
                    orderby u.Nazwisko, u.Imie
                    select new LookupItemDto
                    {
                        Id = u.Id,
                        Nazwa = ((u.Imie ?? "") + " " + (u.Nazwisko ?? "")).Trim()
                    })
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE GetPracownicyKlubuAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

       public async Task<bool> CreateAsync(CreateCzlonkostwoUzytkownikaDto dto)
{
    try
    {
        var rodzaj = dto.RodzajCzlonkostwaId.HasValue
            ? await _context.RodzajeCzlonkostwas
                .FirstOrDefaultAsync(x =>
                    x.RodzajCzlonkostwaId == dto.RodzajCzlonkostwaId.Value)
            : null;

        var entity = new CzlonkostwaUzytkownika
        {
            UzytkownikId = dto.UzytkownikId,
            OfertaKlubuCzlonkostwoId = dto.OfertaKlubuCzlonkostwoId,
            RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId,
            KlubId = dto.KlubId,

            StartAt = dto.StartAt.HasValue
                ? DateOnly.FromDateTime(dto.StartAt.Value)
                : null,

            EndAt = dto.EndAt.HasValue
                ? DateOnly.FromDateTime(dto.EndAt.Value)
                : null,

            Status = string.IsNullOrWhiteSpace(dto.Status)
                ? "AKTYWNE"
                : dto.Status.Trim(),

            CenaZakupu = dto.CenaZakupu,
            ZamowienieId = dto.ZamowienieId,
            NadanePrzezUzytkownikId = dto.NadanePrzezUzytkownikId,
            DataUtworzenia = dto.DataUtworzenia,
            DataAktywacji = dto.DataAktywacji,
            DataAnulowania = dto.DataAnulowania,
            PowodAnulowania = dto.PowodAnulowania?.Trim(),

            LiczbaWejscLimit = rodzaj?.LiczbaWejsc,
            LiczbaWejscWykorzystanych = 0
        };

        _context.CzlonkostwaUzytkownikas.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }
    catch (Exception ex)
    {
        Console.WriteLine("BŁĄD SERVICE CreateAsync CzlonkostwaUzytkownika:");
        Console.WriteLine(ex.ToString());
        throw;
    }
}

        public async Task<bool> UpdateAsync(int id, UpdateCzlonkostwoUzytkownikaDto dto)
        {
            try
            {
                var entity = await _context.CzlonkostwaUzytkownikas
                    .FirstOrDefaultAsync(x => x.CzlonkostwoId == id);

                if (entity == null)
                    return false;

                entity.UzytkownikId = dto.UzytkownikId;
                entity.OfertaKlubuCzlonkostwoId = dto.OfertaKlubuCzlonkostwoId;
                entity.RodzajCzlonkostwaId = dto.RodzajCzlonkostwaId;
                entity.KlubId = dto.KlubId;
                entity.StartAt = dto.StartAt.HasValue ? DateOnly.FromDateTime(dto.StartAt.Value) : null;
                entity.EndAt = dto.EndAt.HasValue ? DateOnly.FromDateTime(dto.EndAt.Value) : null;
                entity.Status = dto.Status?.Trim();
                entity.CenaZakupu = dto.CenaZakupu;
                entity.ZamowienieId = dto.ZamowienieId;
                entity.NadanePrzezUzytkownikId = dto.NadanePrzezUzytkownikId;
                entity.DataUtworzenia = dto.DataUtworzenia;
                entity.DataAktywacji = dto.DataAktywacji;
                entity.DataAnulowania = dto.DataAnulowania;
                entity.PowodAnulowania = dto.PowodAnulowania?.Trim();

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE UpdateAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.CzlonkostwaUzytkownikas
                    .FirstOrDefaultAsync(x => x.CzlonkostwoId == id);

                if (entity == null)
                    return false;

                _context.CzlonkostwaUzytkownikas.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD SERVICE DeleteAsync CzlonkostwaUzytkownika:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<bool> ZakupKarnetuAsync(
    int uzytkownikId,
    int klubId,
    int rodzajCzlonkostwaId)
        {
            var rodzaj = await _context.RodzajeCzlonkostwas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RodzajCzlonkostwaId == rodzajCzlonkostwaId);

            if (rodzaj == null)
                return false;

            var cena = rodzaj.Cena ?? 0m;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var now = DateTime.Now;

                var zamowienie = new Zamowienium
                {
                    NumerZamowienia = $"WWW/{now:yyyyMMddHHmmss}",
                    UzytkownikId = uzytkownikId,
                    KlubId = klubId,
                    Status = "OPLACONE",
                    SumaBrutto = cena,
                    KwotaRabatu = 0,
                    Waluta = "PLN",
                    MetodaPlatnosciId = null,
                    RabatId = null,
                    TypKanalu = "WWW",
                    DataUtworzenia = now,
                    DataOplacenia = now,
                    UtworzylUzytkownikId = null
                };

                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();
                var platnosc = new Platnosci
                {
                    ZamowienieId = zamowienie.ZamowienieId,
                    MetodaPlatnosciId = 1, // lub prawidłowe ID metody płatności
                    Kwota = cena,
                    Waluta = "PLN",
                    Status = "Opłacona",
                    Provider = "WWW",
                    ProviderTransactionId = null,
                    DataPlatnosci = now,
                    DataUtworzenia = now,
                    Uwagi = "Płatność utworzona automatycznie przy zakupie karnetu online."
                };

                _context.Platnoscis.Add(platnosc);

                var pozycja = new PozycjeZamowienium
                {
                    ZamowienieId = zamowienie.ZamowienieId,
                    Typ = "KARNET",
                    RefId = rodzaj.RodzajCzlonkostwaId,
                    Nazwa = rodzaj.Nazwa ?? $"Karnet #{rodzaj.RodzajCzlonkostwaId}",
                    Opis = rodzaj.Opis,
                    Ilosc = 1,
                    CenaJedn = cena,
                    KwotaBrutto = cena,
                };

                _context.PozycjeZamowienia.Add(pozycja);

                var czlonkostwo = new CzlonkostwaUzytkownika
                {
                    UzytkownikId = uzytkownikId,
                    KlubId = klubId,
                    RodzajCzlonkostwaId = rodzajCzlonkostwaId,
                    ZamowienieId = zamowienie.ZamowienieId,

                    Status = "AKTYWNE",
                    CenaZakupu = cena,

                    StartAt = DateOnly.FromDateTime(now),
                    EndAt = rodzaj.OkresDni.HasValue
                        ? DateOnly.FromDateTime(now.AddDays(rodzaj.OkresDni.Value))
                        : null,

                    DataUtworzenia = now,
                    DataAktywacji = now,

                    LiczbaWejscLimit = rodzaj.LiczbaWejsc,
                    LiczbaWejscWykorzystanych = 0
                };

                _context.CzlonkostwaUzytkownikas.Add(czlonkostwo);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<CzlonkostwoUzytkownikaDto>> GetByUzytkownikIdAsync(int uzytkownikId)
        {
            return await _context.CzlonkostwaUzytkownikas
                .AsNoTracking()
                .Include(x => x.RodzajCzlonkostwa)
                .Include(x => x.Klub)
                .Where(x => x.UzytkownikId == uzytkownikId)
                .OrderByDescending(x => x.CzlonkostwoId)
                .Select(x => new CzlonkostwoUzytkownikaDto
                {
                    CzlonkostwoId = x.CzlonkostwoId,
                    UzytkownikId = x.UzytkownikId,

                    RodzajCzlonkostwaId = x.RodzajCzlonkostwaId,
                    RodzajCzlonkostwaNazwa = x.RodzajCzlonkostwa != null
                        ? x.RodzajCzlonkostwa.Nazwa
                        : string.Empty,

                    KlubId = x.KlubId,
                    KlubNazwa = x.Klub != null
                        ? x.Klub.Nazwa
                        : string.Empty,

                    StartAt = x.StartAt.HasValue
                        ? x.StartAt.Value.ToDateTime(TimeOnly.MinValue)
                        : null,

                    EndAt = x.EndAt.HasValue
                        ? x.EndAt.Value.ToDateTime(TimeOnly.MinValue)
                        : null,

                    Status = x.Status,
                    CenaZakupu = x.CenaZakupu,

                    ZamowienieId = x.ZamowienieId,

                    DataUtworzenia = x.DataUtworzenia,
                    DataAktywacji = x.DataAktywacji,

                    LiczbaWejscLimit = x.LiczbaWejscLimit,
                    LiczbaWejscWykorzystanych = x.LiczbaWejscWykorzystanych
                })
                .ToListAsync();
        }

        public async Task<bool> DodajWejscieAsync(int id)
        {
            var entity = await _context.CzlonkostwaUzytkownikas
                .FirstOrDefaultAsync(x => x.CzlonkostwoId == id);

            if (entity == null)
                return false;

            if (entity.LiczbaWejscLimit.HasValue &&
                entity.LiczbaWejscWykorzystanych >= entity.LiczbaWejscLimit.Value)
                return false;

            entity.LiczbaWejscWykorzystanych++;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CofnijWejscieAsync(int id)
        {
            var entity = await _context.CzlonkostwaUzytkownikas
                .FirstOrDefaultAsync(x => x.CzlonkostwoId == id);

            if (entity == null)
                return false;

            if (entity.LiczbaWejscWykorzystanych <= 0)
                return false;

            entity.LiczbaWejscWykorzystanych--;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<int> AnulujPrzeterminowaneAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var items = await _context.CzlonkostwaUzytkownikas
                .Where(x =>
                    x.EndAt.HasValue &&
                    x.EndAt.Value < today &&
                    x.Status == "AKTYWNE")
                .ToListAsync();

            foreach (var item in items)
            {
                item.Status = "ANULOWANE";
                item.DataAnulowania = DateTime.Now;
                item.PowodAnulowania = "Karnet przeterminowany";
            }

            await _context.SaveChangesAsync();

            return items.Count;
        }
    }

}