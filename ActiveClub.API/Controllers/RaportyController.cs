using ActiveClub.API.Services.Interfaces;
using ActiveClub.Shared.DTOs.Raporty;
using Microsoft.AspNetCore.Mvc;

namespace ActiveClub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaportyController : ControllerBase
    {
        private readonly IRaportyService _service;

        public RaportyController(IRaportyService service)
        {
            _service = service;
        }

        [HttpGet("form-data")]
        public async Task<ActionResult<RaportFormDataDto>> GetFormData()
        {
            try
            {
                var data = await _service.GetFormDataAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET FORM DATA Raporty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("definitions")]
        public async Task<ActionResult<List<RaportDefinicjaDto>>> GetDefinitions()
        {
            try
            {
                var items = await _service.GetDefinitionsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET DEFINITIONS Raporty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpGet("generated")]
        public async Task<ActionResult<List<RaportWygenerowanyDto>>> GetGenerated()
        {
            try
            {
                var items = await _service.GetGeneratedAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GET GENERATED Raporty:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPost("generate")]
        public async Task<ActionResult<GenerateRaportResultDto>> Generate(
    [FromBody] GenerateRaportDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("DTO jest null.");

                if (dto.DataOd.HasValue &&
                    dto.DataDo.HasValue &&
                    dto.DataOd > dto.DataDo)
                {
                    return BadRequest("DataOd nie może być większa od DataDo.");
                }

                var result = await _service.GenerateAsync(dto);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD GENERATE Raporty:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}