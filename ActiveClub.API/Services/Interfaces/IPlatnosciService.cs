using ActiveClub.Shared.DTOs.Platnosci;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IPlatnosciService
    {
        Task<List<PlatnoscDto>> GetAllAsync();
        Task<PlatnoscDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreatePlatnoscDto dto);
        Task<bool> UpdateAsync(int id, UpdatePlatnoscDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PlatnoscFormDataDto> GetFormDataAsync();
        
    }
}