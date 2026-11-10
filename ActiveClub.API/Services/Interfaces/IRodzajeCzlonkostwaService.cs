using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;

namespace ActiveClub.API.Services;

public interface IRodzajeCzlonkostwaService
{
    Task<List<RodzajCzlonkostwaDto>> GetAllAsync();
    Task<RodzajCzlonkostwaDto?> GetByIdAsync(int id);
    Task<RodzajCzlonkostwaDto> CreateAsync(CreateRodzajCzlonkostwaDto dto);
    Task<bool> UpdateAsync(int id, UpdateRodzajCzlonkostwaDto dto);
    Task<bool> DeleteAsync(int id);

    Task<bool> ExistsByNameAsync(string nazwa, int? excludeId = null);
    Task<bool> ExistsByCodeAsync(string kod, int? excludeId = null);
}