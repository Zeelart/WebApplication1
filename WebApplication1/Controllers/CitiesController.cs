using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly SupaBaseContext _context;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(SupaBaseContext context, ILogger<CitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetAll()
        {
            var cities = await _context.GetAllCities();
            return Ok(cities.Select(CityResponse.FromModel));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CityResponse>> GetById(int id)
        {
            var city = await _context.GetCityById(id);
            return city == null ? NotFound() : Ok(CityResponse.FromModel(city));
        }

        [HttpPost]
        public async Task<ActionResult<CityResponse>> Create([FromBody] CreateCityRequest request)
        {
            var city = request.ToModel();
            var createdCity = await _context.CreateCity(city);
            return CreatedAtAction(nameof(GetById), new { id = createdCity?.Id }, createdCity != null ? CityResponse.FromModel(createdCity) : null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCityRequest request)
        {
            var existingCity = await _context.GetCityById(id);
            if (existingCity == null) return NotFound();

            request.ApplyTo(existingCity);
            await _context.UpdateCity(existingCity);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _context.DeleteCity(id);

                if (!result)
                {
                    return BadRequest("Cannot delete city - it has associated users. Delete the users first or change their city.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting city {id}");
                return StatusCode(500, "An error occurred while deleting the city");
            }
        }
    }
}