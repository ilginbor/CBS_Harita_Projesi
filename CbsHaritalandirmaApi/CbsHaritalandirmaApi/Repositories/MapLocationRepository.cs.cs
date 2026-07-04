using CbsHaritalandirmaApi.Data;
using CbsHaritalandirmaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CbsHaritalandirmaApi.Repositories
{
    public class MapLocationRepository : IMapLocationRepository
    {
        private readonly AppDbContext _context;

        public MapLocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MapLocation>> GetAllAsync()
        {
            return await _context.MapLocations
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<MapLocation?> GetByIdAsync(int id)
        {
            return await _context.MapLocations.FindAsync(id);
        }

        public async Task<MapLocation> AddAsync(MapLocation mapLocation)
        {
            _context.MapLocations.Add(mapLocation);
            await _context.SaveChangesAsync();
            return mapLocation;
        }

        public async Task<bool> UpdateAsync(MapLocation mapLocation)
        {
            _context.MapLocations.Update(mapLocation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var mapLocation = await _context.MapLocations.FindAsync(id);

            if (mapLocation == null)
            {
                return false;
            }

            _context.MapLocations.Remove(mapLocation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
