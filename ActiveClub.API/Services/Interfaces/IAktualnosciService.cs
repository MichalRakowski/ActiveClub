using ActiveClub.Shared.DTOs.Aktualnosci;

namespace ActiveClub.API.Services;

public interface IAktualnosciService
{
    Task<List<AktualnoscDto>> GetAllAsync();
    Task<AktualnoscDto?> GetByIdAsync(int id);
    Task<AktualnoscDto> CreateAsync(CreateAktualnoscDto dto);
    Task<bool> UpdateAsync(int id, UpdateAktualnoscDto dto);
    Task<bool> DeleteAsync(int id);
}