using CbsHaritalandirmaApi.Models;
using CbsHaritalandirmaApi.Repositories;
using NetTopologySuite.IO;

namespace CbsHaritalandirmaApi.Services
{
    public class MapLocationService : IMapLocationService
    {
        private readonly IMapLocationRepository _repository;

        public MapLocationService(IMapLocationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MapLocation>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MapLocation?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<MapLocation> CreateAsync(MapLocation mapLocation)
        {
            mapLocation.CreatedAt = DateTime.UtcNow;

            var point = new NetTopologySuite.Geometries.Point(
                mapLocation.Longitude,
                mapLocation.Latitude
            )
            {
                SRID = 4326
            };

            var writer = new WKTWriter();
            mapLocation.Wkt = writer.Write(point);
            mapLocation.Geom = point;

            return await _repository.AddAsync(mapLocation);
        }

        public async Task<bool> UpdateAsync(int id, MapLocation updatedMapLocation)
        {
            var existingLocation = await _repository.GetByIdAsync(id);

            if (existingLocation == null)
            {
                return false;
            }

            existingLocation.Name = updatedMapLocation.Name;
            existingLocation.Latitude = updatedMapLocation.Latitude;
            existingLocation.Longitude = updatedMapLocation.Longitude;

            if (existingLocation.CreatedAt.Kind != DateTimeKind.Utc)
            {
                existingLocation.CreatedAt = DateTime.SpecifyKind(
                    existingLocation.CreatedAt,
                    DateTimeKind.Utc
                );
            }

            var point = new NetTopologySuite.Geometries.Point(
                existingLocation.Longitude,
                existingLocation.Latitude
            )
            {
                SRID = 4326
            };

            var writer = new WKTWriter();
            existingLocation.Wkt = writer.Write(point);
            existingLocation.Geom = point;

            await _repository.UpdateAsync(existingLocation);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
