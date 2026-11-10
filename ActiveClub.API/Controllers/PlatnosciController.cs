using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Platnosci;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatnosciController : ControllerBase
    {
        private readonly IPlatnosciService _service;

        public PlatnosciController(IPlatnosciService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlatnoscDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL Platnosci:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlatnoscDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID Platnosci:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<PlatnoscFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA Platnosci:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlatnoscDto dto)
        {
            try
            {
                Console.WriteLine($"API CREATE Platnosc -> ZamowienieId: {dto?.ZamowienieId}, MetodaPlatnosciId: {dto?.MetodaPlatnosciId}, Kwota: {dto?.Kwota}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać płatności.");

                return Ok("Dodano płatność.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE Platnosci:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePlatnoscDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE Platnosc -> ID: {id}, " +
                    $"ZamowienieId: {dto?.ZamowienieId}, " +
                    $"MetodaPlatnosciId: {dto?.MetodaPlatnosciId}, " +
                    $"Kwota: {dto?.Kwota}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono płatności.");

                return Ok("Zaktualizowano płatność.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE Platnosci:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound("Nie znaleziono płatności.");
                return Ok("Usunięto płatność.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE Platnosci:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}