using ActiveClub.Shared.DTOs.Zajecia;

namespace ActiveClub.API.Services.Interfaces;

public interface IZajeciaService
{
    Task<List<ZajecieDto>> GetZajeciaAsync();
}