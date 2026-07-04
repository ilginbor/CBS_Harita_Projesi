using CbsHaritalandirmaApi.Models;

namespace CbsHaritalandirmaApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User> AddAsync(User user);

        Task<List<User>> GetAllAsync();

        Task<User> UpdateAsync(User user);
    }
}