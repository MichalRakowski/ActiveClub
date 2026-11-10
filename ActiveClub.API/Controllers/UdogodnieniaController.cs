using ActiveClub.API.Services;
using ActiveClub.Shared.DTOs.Udogodnienia;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UdogodnieniaController : ControllerBase
{
    private readonly IUdogodnieniaService _service;

    public UdogodnieniaController(IUdogodnieniaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<UdogodnienieDto>>> Get()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }
}