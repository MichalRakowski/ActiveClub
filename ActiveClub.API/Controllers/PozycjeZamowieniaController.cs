using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.PozycjeZamowienia;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PozycjeZamowieniaController : ControllerBase
    {
        private readonly IPozycjeZamowieniaService _service;

        public PozycjeZamowieniaController(IPozycjeZamowieniaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<PozycjaZamowieniaDto>>> GetAll()
        {
            try
            {
                var items = await _service.GetAllAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET ALL PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PozycjaZamowieniaDto>> GetById(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                    return NotFound("Nie znaleziono pozycji zamówienia.");

                return Ok(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET BY ID PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<PozycjaZamowieniaFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePozycjaZamowieniaDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API CREATE PozycjaZamowienia -> ZamowienieId: {dto?.ZamowienieId}, Typ: '{dto?.Typ}', Nazwa: '{dto?.Nazwa}', Ilosc: {dto?.Ilosc}, CenaJedn: {dto?.CenaJedn}, KwotaBrutto: {dto?.KwotaBrutto}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var created = await _service.CreateAsync(dto);

                if (!created)
                    return BadRequest("Nie udało się dodać pozycji zamówienia.");

                return Ok("Dodano pozycję zamówienia.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD CREATE PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePozycjaZamowieniaDto dto)
        {
            try
            {
                Console.WriteLine(
                    $"API UPDATE PozycjaZamowienia -> ID: {id}, ZamowienieId: {dto?.ZamowienieId}, Typ: '{dto?.Typ}', Nazwa: '{dto?.Nazwa}', Ilosc: {dto?.Ilosc}, CenaJedn: {dto?.CenaJedn}, KwotaBrutto: {dto?.KwotaBrutto}");

                if (dto == null)
                    return BadRequest("DTO jest null.");

                var updated = await _service.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound("Nie znaleziono pozycji zamówienia.");

                return Ok("Zaktualizowano pozycję zamówienia.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD UPDATE PozycjeZamowienia:");
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
                    return NotFound("Nie znaleziono pozycji zamówienia.");

                return Ok("Usunięto pozycję zamówienia.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD DELETE PozycjeZamowienia:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}