using ActiveClub.Shared.DTOs.Udogodnienia;

namespace ActiveClub.API.Services;

public interface IUdogodnieniaService
{
    Task<List<UdogodnienieDto>> GetAllAsync();
}