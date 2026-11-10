using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Trenerzy;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class TrenerzyService : ITrenerzyService
    {
        private readonly ActiveClubContext _context;

        public TrenerzyService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrenerDto>> GetAllAsync()
        {
            return await _context.Trenerzies
     .Include(t => t.Uzytkownik)
     .Include(t => t.TrenerKlubs)
         .ThenInclude(tk => tk.Klub)
     .OrderBy(t => t.Nazwisko)
     .ThenBy(t => t.Imie)
     .Select(t => new TrenerDto
     {
         TrenerId = t.TrenerId,
                    UzytkownikId = t.UzytkownikId,

                    Imie = t.Uzytkownik != null ? t.Uzytkownik.Imie : t.Imie,
                    Nazwisko = t.Uzytkownik != null ? t.Uzytkownik.Nazwisko : t.Nazwisko,
                    Email = t.Uzytkownik != null ? t.Uzytkownik.Email : null,
                    Telefon = t.Uzytkownik != null ? t.Uzytkownik.PhoneNumber : null,

                    Bio = t.Bio,
                    SpecjalizacjaOpis = t.SpecjalizacjaOpis,
                    StawkaZaGodzine = t.StawkaZaGodzine,
                    CzyAktywny = t.CzyAktywny,
                    DataUtworzenia = t.DataUtworzenia,
         KlubIds = t.TrenerKlubs
    .Select(tk => tk.KlubId)
    .ToList(),

         KlubyNazwy = string.Join(", ",
    t.TrenerKlubs
        .Where(tk => tk.Klub != null)
        .Select(tk => tk.Klub.Nazwa))
     })
                .ToListAsync();
        }

        public async Task<TrenerDto?> GetByIdAsync(int trenerId)
        {
            return await _context.Trenerzies
                .Include(t => t.Uzytkownik)
                .Include(t => t.TrenerKlubs)
                    .ThenInclude(tk => tk.Klub)
                .Where(t => t.TrenerId == trenerId)
                .Select(t => new TrenerDto
                {
                    TrenerId = t.TrenerId,
                    UzytkownikId = t.UzytkownikId,

                    Imie = t.Uzytkownik != null
                        ? t.Uzytkownik.Imie
                        : t.Imie,

                    Nazwisko = t.Uzytkownik != null
                        ? t.Uzytkownik.Nazwisko
                        : t.Nazwisko,

                    Email = t.Uzytkownik != null
                        ? t.Uzytkownik.Email
                        : null,

                    Telefon = t.Uzytkownik != null
                        ? t.Uzytkownik.PhoneNumber
                        : null,

                    Bio = t.Bio,
                    SpecjalizacjaOpis = t.SpecjalizacjaOpis,
                    StawkaZaGodzine = t.StawkaZaGodzine,
                    CzyAktywny = t.CzyAktywny,
                    DataUtworzenia = t.DataUtworzenia,

                    KlubIds = t.TrenerKlubs
                        .Select(tk => tk.KlubId)
                        .ToList(),

                    KlubyNazwy = string.Join(", ",
                        t.TrenerKlubs
                            .Where(tk => tk.Klub != null)
                            .Select(tk => tk.Klub.Nazwa))
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TrenerDto> CreateAsync(CreateTrenerDto dto)
        {
            var trener = new Trenerzy
            {
                UzytkownikId = dto.UzytkownikId,
                Imie = dto.Imie,
                Nazwisko = dto.Nazwisko,
                Bio = dto.Bio,
                SpecjalizacjaOpis = dto.SpecjalizacjaOpis,
                StawkaZaGodzine = dto.StawkaZaGodzine,
                CzyAktywny = dto.CzyAktywny,
                DataUtworzenia = DateTime.UtcNow
            };

            _context.Trenerzies.Add(trener);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(trener.TrenerId)
                   ?? new TrenerDto
                   {
                       TrenerId = trener.TrenerId,
                       UzytkownikId = trener.UzytkownikId,
                       Imie = trener.Imie,
                       Nazwisko = trener.Nazwisko,
                       Bio = trener.Bio,
                       SpecjalizacjaOpis = trener.SpecjalizacjaOpis,
                       StawkaZaGodzine = trener.StawkaZaGodzine,
                       CzyAktywny = trener.CzyAktywny,
                       DataUtworzenia = trener.DataUtworzenia
                   };
        }

        public async Task<bool> UpdateAsync(int trenerId, UpdateTrenerDto dto)
        {
            var trener = await _context.Trenerzies
                .FirstOrDefaultAsync(t => t.TrenerId == trenerId);

            if (trener == null)
                return false;

            trener.UzytkownikId = dto.UzytkownikId;
            trener.Imie = dto.Imie;
            trener.Nazwisko = dto.Nazwisko;
            trener.Bio = dto.Bio;
            trener.SpecjalizacjaOpis = dto.SpecjalizacjaOpis;
            trener.StawkaZaGodzine = dto.StawkaZaGodzine;
            trener.CzyAktywny = dto.CzyAktywny;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int trenerId)
        {
            var trener = await _context.Trenerzies
                .FirstOrDefaultAsync(t => t.TrenerId == trenerId);

            if (trener == null)
                return false;

            _context.Trenerzies.Remove(trener);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}