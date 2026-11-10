using ActiveClub.Shared.DTOs.MetodyPlatnosci;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IMetodyPlatnosciService
    {
        Task<List<MetodaPlatnosciDto>> GetAllAsync();
        Task<MetodaPlatnosciDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateMetodaPlatnosciDto dto);
        Task<bool> UpdateAsync(int id, UpdateMetodaPlatnosciDto dto);
        Task<bool> DeleteAsync(int id);
    }
}