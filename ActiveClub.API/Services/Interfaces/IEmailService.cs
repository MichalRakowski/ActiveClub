using ActiveClub.Shared.DTOs.Kontakt;

public interface IEmailService
{
    Task SendKontaktAsync(KontaktFormDto dto);
}