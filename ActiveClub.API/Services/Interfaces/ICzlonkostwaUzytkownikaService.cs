using ActiveClub.Shared.DTOs.Common;
using ActiveClub.Shared.DTOs.CzlonkostwaUzytkownika;

namespace ActiveClub.API.Services.Interfaces
{
    public interface ICzlonkostwaUzytkownikaService
    {
        Task<List<CzlonkostwoUzytkownikaDto>> GetAllAsync();
        Task<CzlonkostwoUzytkownikaDto?> GetByIdAsync(int id);
        Task<CzlonkostwoUzytkownikaFormDataDto> GetFormDataAsync();
        Task<List<LookupItemDto>> GetPracownicyKlubuAsync(int klubId);
        Task<bool> CreateAsync(CreateCzlonkostwoUzytkownikaDto dto);
        Task<bool> UpdateAsync(int id, UpdateCzlonkostwoUzytkownikaDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> ZakupKarnetuAsync(
            int uzytkownikId,
            int klubId,
            int rodzajCzlonkostwaId);

        Task<List<CzlonkostwoUzytkownikaDto>> GetByUzytkownikIdAsync(int uzytkownikId);

        Task<bool> DodajWejscieAsync(int id);

        Task<bool> CofnijWejscieAsync(int id);
        Task<int> AnulujPrzeterminowaneAsync();
    }
}