using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Opinie;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpinieController : ControllerBase
    {
        private readonly IOpinieService _service;

        public OpinieController(IOpinieService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<OpiniaDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Opinie:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OpiniaDto>> GetById(int id)
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
                Console.WriteLine("BŁĄD GET BY ID Opinie:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOpiniaDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE Opinia -> ID: {id}, KlubId: {dto?.KlubId}, UzytkownikId: {dto?.UzytkownikId}, Ocena: {dto?.Ocena}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono opinii.");

                return Ok("Zaktualizowano opinię.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Opinie:");
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
                    return NotFound("Nie znaleziono opinii.");

                return Ok("Usunięto opinię.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Opinie:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}