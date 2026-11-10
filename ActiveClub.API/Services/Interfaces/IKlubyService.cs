using ActiveClub.Shared.DTOs.Kluby;

namespace ActiveClub.API.Services;

public interface IKlubyService
{
    Task<List<KlubDto>> GetAsync(string? q, string? miasto);
    Task<List<string>> GetMiastaAsync();
    Task<KlubDto?> GetByIdAsync(int id);
    Task<KlubDto> CreateAsync(CreateKlubDto dto);
    Task<KlubDto?> UpdateAsync(UpdateKlubDto dto);
    Task<bool> DeleteAsync(int id);
}