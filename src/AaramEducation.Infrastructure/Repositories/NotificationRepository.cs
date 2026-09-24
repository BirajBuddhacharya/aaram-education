using AaramEducation.Core.Entities;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class NotificationRepository(ApplicationDbContext db) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetRecentAsync(int userId, int count = 10) =>
        await db.Notifications.AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();

    public Task<int> GetUnreadCountAsync(int userId) =>
        db.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task CreateAsync(int userId, string title, string message)
    {
        db.Notifications.Add(new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
        });
        await db.SaveChangesAsync();
    }

    public async Task MarkReadAsync(int notificationId, int userId)
    {
        var n = await db.Notifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId && x.UserId == userId);
        if (n is null) return;
        n.IsRead = true;
        await db.SaveChangesAsync();
    }

    public async Task MarkAllReadAsync(int userId)
    {
        await db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
    }
}
