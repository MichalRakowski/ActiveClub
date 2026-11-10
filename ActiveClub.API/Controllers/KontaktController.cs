using ActiveClub.Shared.DTOs.Kontakt;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class KontaktController : ControllerBase
{
    private readonly IEmailService _emailService;

    public KontaktController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] KontaktFormDto dto)
    {
        await _emailService.SendKontaktAsync(dto);
        return Ok("Wiadomość została wysłana.");
    }
}