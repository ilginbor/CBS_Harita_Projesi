using CbsHaritalandirmaApi.Models;
using NetTopologySuite.Geometries;

namespace CbsHaritalandirmaApi.Repositories
{
    public interface IDrawGeometryRepository
    {
        Task<DrawPoint> AddPointAsync(DrawPoint point);

        Task<DrawLine> AddLineAsync(DrawLine line);

        Task<DrawPolygon> AddPolygonAsync(DrawPolygon polygon);

        Task<List<DrawPoint>> GetPointsAsync();

        Task<List<DrawLine>> GetLinesAsync();

        Task<List<DrawPolygon>> GetPolygonsAsync();

        Task<List<DrawPoint>> GetPointsByUserIdAsync(int userId);

        Task<List<DrawLine>> GetLinesByUserIdAsync(int userId);

        Task<List<DrawPolygon>> GetPolygonsByUserIdAsync(int userId);

        Task<int> CountObjectsInsidePolygonAsync(Polygon polygon, int userId);

        Task<bool> SoftDeletePointAsync(int id);

        Task<bool> SoftDeleteLineAsync(int id);

        Task<bool> SoftDeletePolygonAsync(int id);
    }
}