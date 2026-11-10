using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.PracownicyKlubu;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PracownicyKlubuController : ControllerBase
    {
        private readonly IPracownicyKlubuService _service;

        public PracownicyKlubuController(IPracownicyKlubuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PracownikKlubuDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PracownikKlubuDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                if (item == null) return NotFound();
                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<PracownikKlubuFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     [FromBody] CreatePracownikKlubuDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest(
                        "DataOd nie może być większa od DataDo.");
                }

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest(
                        "Nie udało się dodać pracownika klubu.");

                return Ok("Dodano pracownika klubu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE PracownicyKlubu:");
                Console.WriteLine(ex.ToString());

                return StatusCode(
                    500,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
     int id,
     [FromBody] UpdatePracownikKlubuDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest(
                        "DataOd nie może być większa od DataDo.");
                }

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound(
                        "Nie znaleziono pracownika klubu.");

                return Ok("Zaktualizowano pracownika klubu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE PracownicyKlubu:");
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
                if (!deleted) return NotFound("Nie znaleziono pracownika klubu.");
                return Ok("Usunięto pracownika klubu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE PracownicyKlubu:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}