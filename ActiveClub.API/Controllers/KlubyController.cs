using ActiveClub.API.Services;
using ActiveClub.Shared.DTOs.Kluby;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KlubyController : ControllerBase
{
    private readonly IKlubyService _service;

    public KlubyController(IKlubyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<KlubDto>>> Get([FromQuery] string? q, [FromQuery] string? miasto)
    {
        try
        {
            var kluby = await _service.GetAsync(q, miasto);
            return Ok(kluby);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas pobierania klubów.");
        }
    }

    [HttpGet("miasta")]
    public async Task<ActionResult<List<string>>> GetMiasta()
    {
        try
        {
            var miasta = await _service.GetMiastaAsync();
            return Ok(miasta);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas pobierania miast.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KlubDto>> GetById(int id)
    {
        try
        {
            var klub = await _service.GetByIdAsync(id);

            if (klub == null)
                return NotFound($"Nie znaleziono klubu o id = {id}.");

            return Ok(klub);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas pobierania klubu.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<KlubDto>> Create([FromBody] CreateKlubDto dto)
    {
        try
        {
            if (dto == null)
                return BadRequest("Brak danych klubu.");

            

            var createdKlub = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = createdKlub.KlubId }, createdKlub);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas tworzenia klubu.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<KlubDto>> Update(int id, [FromBody] UpdateKlubDto dto)
    {
        try
        {
            if (dto == null)
                return BadRequest("Brak danych klubu.");

            if (id != dto.KlubId)
                return BadRequest("Id w adresie i w obiekcie nie są zgodne.");

           

            var updatedKlub = await _service.UpdateAsync(dto);

            if (updatedKlub == null)
                return NotFound($"Nie znaleziono klubu o id = {id}.");

            return Ok(updatedKlub);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas aktualizacji klubu.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound($"Nie znaleziono klubu o id = {id}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Błąd podczas usuwania klubu.");
        }
    }
}