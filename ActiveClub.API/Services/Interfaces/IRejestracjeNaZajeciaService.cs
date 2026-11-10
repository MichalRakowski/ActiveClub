using ActiveClub.Shared.DTOs.RejestracjeNaZajecia;

namespace ActiveClub.API.Services.Interfaces
{
    public interface IRejestracjeNaZajeciaService
    {
        Task<List<RejestracjaNaZajeciaDto>> GetAllAsync();
        Task<RejestracjaNaZajeciaDto?> GetByIdAsync(int id);
        Task<RejestracjaNaZajeciaFormDataDto> GetFormDataAsync();
        Task<bool> CreateAsync(CreateRejestracjaNaZajeciaDto dto);
        Task<bool> UpdateAsync(int id, UpdateRejestracjaNaZajeciaDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<RejestracjaNaZajeciaDto>> GetByUzytkownikIdAsync(int uzytkownikId);
    }
}