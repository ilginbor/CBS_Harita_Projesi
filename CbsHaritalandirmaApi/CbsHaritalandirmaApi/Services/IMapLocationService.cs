using CbsHaritalandirmaApi.Models;

namespace CbsHaritalandirmaApi.Services
{
    public interface IMapLocationService
    {
        Task<List<MapLocation>> GetAllAsync();
        Task<MapLocation?> GetByIdAsync(int id);
        Task<MapLocation> CreateAsync(MapLocation mapLocation);
        Task<bool> UpdateAsync(int id, MapLocation updatedMapLocation);
        Task<bool> DeleteAsync(int id);
    }
}
