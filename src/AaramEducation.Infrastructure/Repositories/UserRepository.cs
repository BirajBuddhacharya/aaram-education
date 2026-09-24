using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public Task<User?> GetByIdAsync(int userId) =>
        db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);

    public Task<User?> GetByEmailAsync(string email) =>
        db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await db.Users.AsNoTracking().OrderBy(u => u.LastName).ToListAsync();

    public async Task<User> CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await db.Users.FindAsync(userId);
        if (user is not null)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
        }
    }

    public Task<bool> EmailExistsAsync(string email) =>
        db.Users.AnyAsync(u => u.Email == email);
}
