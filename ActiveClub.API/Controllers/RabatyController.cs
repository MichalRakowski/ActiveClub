using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Rabaty;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RabatyController : ControllerBase
    {
        private readonly IRabatyService _service;

        public RabatyController(IRabatyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<RabatDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Rabaty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RabatDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID Rabaty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRabatDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API CREATE Rabat -> Kod: '{dto?.Kod}', Typ: '{dto?.Typ}', Wartosc: {dto?.Wartosc}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest("DataOd nie może być większa od DataDo.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać rabatu.");

                return Ok("Dodano rabat.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE Rabaty:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRabatDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE Rabat -> ID: {id}, Kod: '{dto?.Kod}', Typ: '{dto?.Typ}', Wartosc: {dto?.Wartosc}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest("DataOd nie może być większa od DataDo.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono rabatu.");

                return Ok("Zaktualizowano rabat.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Rabaty:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);

                if (!deleted)
                    return NotFound("Nie znaleziono rabatu.");

                return Ok("Usunięto rabat.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Rabaty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}