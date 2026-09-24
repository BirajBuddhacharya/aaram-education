using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) { _db = db; }

        public Task<User?> GetByIdAsync(int userId) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);

        public Task<User?> GetByEmailAsync(string email) =>
            _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _db.Users.AsNoTracking().OrderBy(u => u.LastName).ToListAsync();

        public async Task<User> CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user is not null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
        }

        public Task<bool> EmailExistsAsync(string email) =>
            _db.Users.AnyAsync(u => u.Email == email);
    }
}
