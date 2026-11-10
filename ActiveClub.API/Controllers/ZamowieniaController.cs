using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Zamowienia;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZamowieniaController : ControllerBase
    {
        private readonly IZamowieniaService _service;

        public ZamowieniaController(IZamowieniaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ZamowienieDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Zamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ZamowienieDto>> GetById(int id)
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
                Console.WriteLine("BŁĄD GET BY ID Zamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<ZamowienieFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA Zamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateZamowienieDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOplacenia.HasValue &&
                    dto.DataOplacenia < dto.DataUtworzenia)
                {
                    return BadRequest("Data opłacenia nie może być wcześniejsza niż data utworzenia.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać zamówienia.");

                return Ok("Dodano zamówienie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE Zamowienia:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateZamowienieDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOplacenia.HasValue &&
                    dto.DataOplacenia < dto.DataUtworzenia)
                {
                    return BadRequest("Data opłacenia nie może być wcześniejsza niż data utworzenia.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono zamówienia.");

                return Ok("Zaktualizowano zamówienie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Zamowienia:");
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
                if (!deleted) return NotFound("Nie znaleziono zamówienia.");

                return Ok("Usunięto zamówienie.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Zamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}