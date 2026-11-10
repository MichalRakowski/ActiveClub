using ActiveClub.Shared.DTOs.GodzinyOtwarcia;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IGodzinyOtwarciaService
    {
        Task<List<GodzinyOtwarciaDto>> GetAllAsync();
        Task<List<GodzinyOtwarciaDto>> GetByKlubIdAsync(int klubId);
        Task<GodzinyOtwarciaDto?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateGodzinyOtwarciaDto dto);
        Task<bool> UpdateAsync(int id, UpdateGodzinyOtwarciaDto dto);
        Task<bool> DeleteAsync(int id);
    }
}