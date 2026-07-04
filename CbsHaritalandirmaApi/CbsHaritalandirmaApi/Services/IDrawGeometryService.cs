using CbsHaritalandirmaApi.Dtos;

namespace CbsHaritalandirmaApi.Services
{
    public interface IDrawGeometryService
    {
        Task<DrawGeometryResponseDto> SaveGeometryAsync(DrawGeometryDto dto);

        Task<List<DrawGeometryResponseDto>> GetAllGeometriesAsync();

        Task<List<DrawGeometryResponseDto>> GetPointsAsync();

        Task<List<DrawGeometryResponseDto>> GetLinesAsync();

        Task<List<DrawGeometryResponseDto>> GetPolygonsAsync();

        Task<List<DrawGeometryResponseDto>> GetAllGeometriesByUserIdAsync(int userId);

        Task<bool> SoftDeleteGeometryAsync(string geometryType, int id);
    }
}