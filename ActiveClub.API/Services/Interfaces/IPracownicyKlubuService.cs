using ActiveClub.Shared.DTOs.PracownicyKlubu;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IPracownicyKlubuService
    {
        Task<List<PracownikKlubuDto>> GetAllAsync();
        Task<PracownikKlubuDto?> GetByIdAsync(int id);
        Task<PracownikKlubuFormDataDto> GetFormDataAsync();
        Task<bool> CreateAsync(CreatePracownikKlubuDto dto);
        Task<bool> UpdateAsync(int id, UpdatePracownikKlubuDto dto);
        Task<bool> DeleteAsync(int id);
    }
}