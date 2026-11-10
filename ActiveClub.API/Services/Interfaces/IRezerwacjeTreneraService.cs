using ActiveClub.Shared.DTOs.RezerwacjeTrenera;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IRezerwacjeTreneraService
    {
        Task<IEnumerable<RezerwacjaTreneraDto>> GetAllAsync();
        Task<IEnumerable<RezerwacjaTreneraDto>> GetByUzytkownikIdAsync(int uzytkownikId);
        Task<RezerwacjaTreneraDto?> GetByIdAsync(int rezerwacjaId);
        Task<RezerwacjaTreneraDto> CreateAsync(CreateRezerwacjaTreneraDto dto);
        Task<bool> UpdateAsync(int rezerwacjaId, UpdateRezerwacjaTreneraDto dto);
        Task<bool> DeleteAsync(int rezerwacjaId);
    }
}