using CbsHaritalandirmaApi.Dtos;
using CbsHaritalandirmaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CbsHaritalandirmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawGeometryController : ControllerBase
    {
        private readonly IDrawGeometryService _drawGeometryService;

        public DrawGeometryController(IDrawGeometryService drawGeometryService)
        {
            _drawGeometryService = drawGeometryService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveGeometry(DrawGeometryDto dto)
        {
            try
            {
                var result = await _drawGeometryService.SaveGeometryAsync(dto);

                return Ok(new
                {
                    success = true,
                    message = "Geometri başarıyla kaydedildi.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGeometries()
        {
            try
            {
                var result = await _drawGeometryService.GetAllGeometriesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGeometriesByUserId(int userId)
        {
            try
            {
                var result = await _drawGeometryService.GetAllGeometriesByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("points")]
        public async Task<IActionResult> GetPoints()
        {
            try
            {
                var result = await _drawGeometryService.GetPointsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("lines")]
        public async Task<IActionResult> GetLines()
        {
            try
            {
                var result = await _drawGeometryService.GetLinesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("polygons")]
        public async Task<IActionResult> GetPolygons()
        {
            try
            {
                var result = await _drawGeometryService.GetPolygonsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpDelete("delete/{geometryType}/{id}")]
        public async Task<IActionResult> SoftDeleteGeometry(string geometryType, int id)
        {
            try
            {
                var result = await _drawGeometryService.SoftDeleteGeometryAsync(geometryType, id);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Silinecek geometri bulunamadı."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Geometri başarıyla silindi."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}