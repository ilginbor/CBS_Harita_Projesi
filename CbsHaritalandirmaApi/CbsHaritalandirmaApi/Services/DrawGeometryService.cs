using CbsHaritalandirmaApi.Dtos;
using CbsHaritalandirmaApi.Models;
using CbsHaritalandirmaApi.Repositories;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace CbsHaritalandirmaApi.Services
{
    public class DrawGeometryService : IDrawGeometryService
    {
        private readonly IDrawGeometryRepository _repository;

        public DrawGeometryService(IDrawGeometryRepository repository)
        {
            _repository = repository;
        }

        public async Task<DrawGeometryResponseDto> SaveGeometryAsync(DrawGeometryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Wkt))
            {
                throw new Exception("WKT bilgisi boş olamaz.");
            }

            if (string.IsNullOrWhiteSpace(dto.GeometryType))
            {
                throw new Exception("GeometryType bilgisi boş olamaz.");
            }

            var reader = new WKTReader();
            var geometry = reader.Read(dto.Wkt);
            geometry.SRID = 4326;

            var geometryType = dto.GeometryType.Trim().ToLower();
            var color = string.IsNullOrWhiteSpace(dto.Color) ? GetDefaultColor(geometryType) : dto.Color;
            var description = dto.Description;

            if (geometryType == "point")
            {
                var point = new DrawPoint
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    Wkt = dto.Wkt,
                    Geom = (Point)geometry,
                    Color = color,
                    Description = description,
                    ObjectCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDelete = false
                };

                var savedPoint = await _repository.AddPointAsync(point);

                return ToDto(savedPoint, "Point");
            }

            if (geometryType == "linestring" || geometryType == "line")
            {
                var line = new DrawLine
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    Wkt = dto.Wkt,
                    Geom = (LineString)geometry,
                    Color = color,
                    Description = description,
                    ObjectCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDelete = false
                };

                var savedLine = await _repository.AddLineAsync(line);

                return ToDto(savedLine, "LineString");
            }

            if (geometryType == "polygon")
            {
                var polygonGeometry = (Polygon)geometry;

                var userId = dto.UserId ?? 0;

                var objectCount = userId > 0
                    ? await _repository.CountObjectsInsidePolygonAsync(polygonGeometry, userId)
                    : 0;

                var polygon = new DrawPolygon
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    Wkt = dto.Wkt,
                    Geom = polygonGeometry,
                    Color = color,
                    Description = description,
                    ObjectCount = objectCount,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDelete = false
                };

                var savedPolygon = await _repository.AddPolygonAsync(polygon);

                return ToDto(savedPolygon, "Polygon");
            }

            throw new Exception("Desteklenmeyen geometri tipi.");
        }

        public async Task<List<DrawGeometryResponseDto>> GetAllGeometriesAsync()
        {
            var result = new List<DrawGeometryResponseDto>();

            result.AddRange(await GetPointsAsync());
            result.AddRange(await GetLinesAsync());
            result.AddRange(await GetPolygonsAsync());

            return result.OrderBy(x => x.Id).ToList();
        }

        public async Task<List<DrawGeometryResponseDto>> GetPointsAsync()
        {
            var points = await _repository.GetPointsAsync();

            return points.Select(x => ToDto(x, "Point")).ToList();
        }

        public async Task<List<DrawGeometryResponseDto>> GetLinesAsync()
        {
            var lines = await _repository.GetLinesAsync();

            return lines.Select(x => ToDto(x, "LineString")).ToList();
        }

        public async Task<List<DrawGeometryResponseDto>> GetPolygonsAsync()
        {
            var polygons = await _repository.GetPolygonsAsync();

            return polygons.Select(x => ToDto(x, "Polygon")).ToList();
        }

        public async Task<List<DrawGeometryResponseDto>> GetAllGeometriesByUserIdAsync(int userId)
        {
            var result = new List<DrawGeometryResponseDto>();

            var points = await _repository.GetPointsByUserIdAsync(userId);
            var lines = await _repository.GetLinesByUserIdAsync(userId);
            var polygons = await _repository.GetPolygonsByUserIdAsync(userId);

            result.AddRange(points.Select(x => ToDto(x, "Point")));
            result.AddRange(lines.Select(x => ToDto(x, "LineString")));
            result.AddRange(polygons.Select(x => ToDto(x, "Polygon")));

            return result.OrderBy(x => x.Id).ToList();
        }

        public async Task<bool> SoftDeleteGeometryAsync(string geometryType, int id)
        {
            if (string.IsNullOrWhiteSpace(geometryType))
            {
                throw new Exception("GeometryType bilgisi boş olamaz.");
            }

            var type = geometryType.Trim().ToLower();

            if (type == "point")
            {
                return await _repository.SoftDeletePointAsync(id);
            }

            if (type == "linestring" || type == "line")
            {
                return await _repository.SoftDeleteLineAsync(id);
            }

            if (type == "polygon")
            {
                return await _repository.SoftDeletePolygonAsync(id);
            }

            throw new Exception("Desteklenmeyen geometri tipi.");
        }

        private string GetDefaultColor(string geometryType)
        {
            if (geometryType == "point")
            {
                return "#2563eb";
            }

            if (geometryType == "linestring" || geometryType == "line")
            {
                return "#16a34a";
            }

            if (geometryType == "polygon")
            {
                return "#dc2626";
            }

            return "#2563eb";
        }

        private DrawGeometryResponseDto ToDto(DrawPoint point, string geometryType)
        {
            return new DrawGeometryResponseDto
            {
                Id = point.Id,
                UserId = point.UserId,
                Name = point.Name,
                Color = point.Color,
                Description = point.Description,
                ObjectCount = point.ObjectCount,
                GeometryType = geometryType,
                Wkt = point.Wkt,
                CreatedAt = point.CreatedAt,
                IsActive = point.IsActive,
                IsDelete = point.IsDelete
            };
        }

        private DrawGeometryResponseDto ToDto(DrawLine line, string geometryType)
        {
            return new DrawGeometryResponseDto
            {
                Id = line.Id,
                UserId = line.UserId,
                Name = line.Name,
                Color = line.Color,
                Description = line.Description,
                ObjectCount = line.ObjectCount,
                GeometryType = geometryType,
                Wkt = line.Wkt,
                CreatedAt = line.CreatedAt,
                IsActive = line.IsActive,
                IsDelete = line.IsDelete
            };
        }

        private DrawGeometryResponseDto ToDto(DrawPolygon polygon, string geometryType)
        {
            return new DrawGeometryResponseDto
            {
                Id = polygon.Id,
                UserId = polygon.UserId,
                Name = polygon.Name,
                Color = polygon.Color,
                Description = polygon.Description,
                ObjectCount = polygon.ObjectCount,
                GeometryType = geometryType,
                Wkt = polygon.Wkt,
                CreatedAt = polygon.CreatedAt,
                IsActive = polygon.IsActive,
                IsDelete = polygon.IsDelete
            };
        }
    }
}