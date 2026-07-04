using CbsHaritalandirmaApi.Data;
using CbsHaritalandirmaApi.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace CbsHaritalandirmaApi.Repositories
{
    public class DrawGeometryRepository : IDrawGeometryRepository
    {
        private readonly AppDbContext _context;

        public DrawGeometryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DrawPoint> AddPointAsync(DrawPoint point)
        {
            point.CreatedAt = DateTime.UtcNow;
            point.IsActive = true;
            point.IsDelete = false;

            _context.DrawPoints.Add(point);
            await _context.SaveChangesAsync();

            return point;
        }

        public async Task<DrawLine> AddLineAsync(DrawLine line)
        {
            line.CreatedAt = DateTime.UtcNow;
            line.IsActive = true;
            line.IsDelete = false;

            _context.DrawLines.Add(line);
            await _context.SaveChangesAsync();

            return line;
        }

        public async Task<DrawPolygon> AddPolygonAsync(DrawPolygon polygon)
        {
            polygon.CreatedAt = DateTime.UtcNow;
            polygon.IsActive = true;
            polygon.IsDelete = false;

            _context.DrawPolygons.Add(polygon);
            await _context.SaveChangesAsync();

            return polygon;
        }

        public async Task<List<DrawPoint>> GetPointsAsync()
        {
            return await _context.DrawPoints
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DrawLine>> GetLinesAsync()
        {
            return await _context.DrawLines
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DrawPolygon>> GetPolygonsAsync()
        {
            return await _context.DrawPolygons
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DrawPoint>> GetPointsByUserIdAsync(int userId)
        {
            return await _context.DrawPoints
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DrawLine>> GetLinesByUserIdAsync(int userId)
        {
            return await _context.DrawLines
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DrawPolygon>> GetPolygonsByUserIdAsync(int userId)
        {
            return await _context.DrawPolygons
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<int> CountObjectsInsidePolygonAsync(Polygon polygon, int userId)
        {
            if (polygon == null)
            {
                return 0;
            }

            var points = await _context.DrawPoints
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .ToListAsync();

            var lines = await _context.DrawLines
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .ToListAsync();

            var polygons = await _context.DrawPolygons
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDelete)
                .ToListAsync();

            var pointCount = points.Count(x =>
                x.Geom != null &&
                (polygon.Contains(x.Geom) || polygon.Covers(x.Geom) || polygon.Intersects(x.Geom))
            );

            var lineCount = lines.Count(x =>
                x.Geom != null &&
                polygon.Intersects(x.Geom)
            );

            var polygonCount = polygons.Count(x =>
                x.Geom != null &&
                polygon.Intersects(x.Geom)
            );

            return pointCount + lineCount + polygonCount;
        }

        public async Task<bool> SoftDeletePointAsync(int id)
        {
            var point = await _context.DrawPoints.FindAsync(id);

            if (point == null)
            {
                return false;
            }

            point.IsActive = false;
            point.IsDelete = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteLineAsync(int id)
        {
            var line = await _context.DrawLines.FindAsync(id);

            if (line == null)
            {
                return false;
            }

            line.IsActive = false;
            line.IsDelete = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeletePolygonAsync(int id)
        {
            var polygon = await _context.DrawPolygons.FindAsync(id);

            if (polygon == null)
            {
                return false;
            }

            polygon.IsActive = false;
            polygon.IsDelete = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}