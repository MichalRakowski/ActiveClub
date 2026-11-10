using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.UstawieniaKontaktowe;
using ActiveClubData.Data;
using ActiveClubData.Models;
using Microsoft.EntityFrameworkCore;

namespace ActiveClub.API.Services
{
    public class UstawieniaKontaktoweService : IUstawieniaKontaktoweService
    {
        private readonly ActiveClubContext _context;

        public UstawieniaKontaktoweService(ActiveClubContext context)
        {
            _context = context;
        }

        public async Task<UstawieniaKontaktoweDto> GetAsync()
        {
            var item = await _context.UstawieniaKontaktowes.FirstOrDefaultAsync();

            if (item == null)
            {
                item = new UstawieniaKontaktowe
                {
                    Email = "kontakt@activeclub.pl",
                    Telefon = "+48 123 456 789",
                    Adres = "ul. Sportowa 10, 00-001 Warszawa",
                    GodzinyKontaktu = "Poniedziałek – Piątek: 8:00 – 18:00",
                    DataModyfikacji = DateTime.Now
                };

                _context.UstawieniaKontaktowes.Add(item);
                await _context.SaveChangesAsync();
            }

            return new UstawieniaKontaktoweDto
            {
                UstawieniaKontaktoweId = item.UstawieniaKontaktoweId,
                Email = item.Email,
                Telefon = item.Telefon,
                Adres = item.Adres,
                GodzinyKontaktu = item.GodzinyKontaktu,
                DataModyfikacji = item.DataModyfikacji
            };
        }

        public async Task<bool> UpdateAsync(UpdateUstawieniaKontaktoweDto dto)
        {
            var item = await _context.UstawieniaKontaktowes.FirstOrDefaultAsync();

            if (item == null)
            {
                item = new UstawieniaKontaktowe();
                _context.UstawieniaKontaktowes.Add(item);
            }

            item.Email = dto.Email.Trim();
            item.Telefon = dto.Telefon.Trim();
            item.Adres = dto.Adres.Trim();
            item.GodzinyKontaktu = dto.GodzinyKontaktu.Trim();
            item.DataModyfikacji = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}