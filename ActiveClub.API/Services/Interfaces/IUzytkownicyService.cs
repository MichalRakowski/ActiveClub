using ActiveClub.Shared.DTOs.Uzytkownicy;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IUzytkownicyService
    {
        Task<List<UzytkownikDto>> GetAllAsync();
        Task<UzytkownikDto?> GetByIdAsync(int id);
        Task<(bool Succeeded, IEnumerable<string> Errors, UzytkownikDto? User)> CreateAsync(CreateUzytkownikDto dto);
        Task<(bool Succeeded, IEnumerable<string> Errors)> UpdateAsync(int id, UpdateUzytkownikDto dto);
        Task<bool> DeleteAsync(int id);
        
    }
}