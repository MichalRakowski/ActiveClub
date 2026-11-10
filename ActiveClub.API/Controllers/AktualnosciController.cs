using ActiveClub.API.Services;
using ActiveClub.Shared.DTOs.Aktualnosci;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AktualnosciController : ControllerBase
{
    private readonly IAktualnosciService _service;

    public AktualnosciController(IAktualnosciService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<AktualnoscDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AktualnoscDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<AktualnoscDto>> Create(
    [FromBody] CreateAktualnoscDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.AktualnoscId },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdateAktualnoscDto dto)
    {
        if (id != dto.AktualnoscId)
        {
            return BadRequest(
                "Id w adresie i w DTO musi być takie samo.");
        }

        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}