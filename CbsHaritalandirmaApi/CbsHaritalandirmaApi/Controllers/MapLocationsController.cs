using CbsHaritalandirmaApi.Models;
using CbsHaritalandirmaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CbsHaritalandirmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapLocationsController : ControllerBase
    {
        private readonly IMapLocationService _service;

        public MapLocationsController(IMapLocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MapLocation>>> GetMapLocations()
        {
            var mapLocations = await _service.GetAllAsync();
            return Ok(mapLocations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MapLocation>> GetMapLocation(int id)
        {
            var mapLocation = await _service.GetByIdAsync(id);

            if (mapLocation == null)
            {
                return NotFound();
            }

            return Ok(mapLocation);
        }

        [HttpPost]
        public async Task<ActionResult<MapLocation>> CreateMapLocation(MapLocation mapLocation)
        {
            var createdLocation = await _service.CreateAsync(mapLocation);

            return CreatedAtAction(
                nameof(GetMapLocation),
                new { id = createdLocation.Id },
                createdLocation
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMapLocation(int id, MapLocation updatedMapLocation)
        {
            var result = await _service.UpdateAsync(id, updatedMapLocation);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMapLocation(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}