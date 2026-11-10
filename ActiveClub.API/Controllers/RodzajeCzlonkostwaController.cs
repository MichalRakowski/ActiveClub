using ActiveClub.API.Services;
using ActiveClub.Shared.DTOs.RodzajeCzlonkostwa;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RodzajeCzlonkostwaController : ControllerBase
{
    private readonly IRodzajeCzlonkostwaService _service;

    public RodzajeCzlonkostwaController(IRodzajeCzlonkostwaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<RodzajCzlonkostwaDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RodzajCzlonkostwaDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<RodzajCzlonkostwaDto>> Create(
        [FromBody] CreateRodzajCzlonkostwaDto dto)
    {
        var existsByName = await _service.ExistsByNameAsync(dto.Nazwa);

        if (existsByName)
            return BadRequest("Rodzaj członkostwa o takiej nazwie już istnieje.");

        var created = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.RodzajCzlonkostwaId },
            created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateRodzajCzlonkostwaDto dto)
    {
        if (id != dto.RodzajCzlonkostwaId)
            return BadRequest("Id w adresie i DTO musi być takie samo.");

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