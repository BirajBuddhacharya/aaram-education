using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetRecentAsync(int userId, int count = 10);
    Task<int> GetUnreadCountAsync(int userId);
    Task CreateAsync(int userId, string title, string message);
    Task MarkReadAsync(int notificationId, int userId);
    Task MarkAllReadAsync(int userId);
}
