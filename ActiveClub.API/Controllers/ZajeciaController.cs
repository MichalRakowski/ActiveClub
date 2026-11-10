using ActiveClub.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZajeciaController : ControllerBase
{
    private readonly IZajeciaService _zajeciaService;

    public ZajeciaController(IZajeciaService zajeciaService)
    {
        _zajeciaService = zajeciaService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _zajeciaService.GetZajeciaAsync();
        return Ok(result);
    }
}