using CbsHaritalandirmaApi.Models;

namespace CbsHaritalandirmaApi.Repositories
{
    public interface IMapLocationRepository
    {
        Task<List<MapLocation>> GetAllAsync();
        Task<MapLocation?> GetByIdAsync(int id);
        Task<MapLocation> AddAsync(MapLocation mapLocation);
        Task<bool> UpdateAsync(MapLocation mapLocation);
        Task<bool> DeleteAsync(int id);
    }
}
