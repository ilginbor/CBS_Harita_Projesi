using CbsHaritalandirmaApi.Data;
using CbsHaritalandirmaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CbsHaritalandirmaApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.IsActive &&
                    !x.IsDelete
                );
        }

        public async Task<User> AddAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.ModifiedAt = null;
            user.LastLoginAt = null;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<User> UpdateAsync(User user)
        {
            NormalizeUserDates(user);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void NormalizeUserDates(User user)
        {
            user.CreatedAt = ToUtc(user.CreatedAt);

            if (user.ModifiedAt.HasValue)
            {
                user.ModifiedAt = ToUtc(user.ModifiedAt.Value);
            }

            if (user.LastLoginAt.HasValue)
            {
                user.LastLoginAt = ToUtc(user.LastLoginAt.Value);
            }
        }

        private DateTime ToUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
