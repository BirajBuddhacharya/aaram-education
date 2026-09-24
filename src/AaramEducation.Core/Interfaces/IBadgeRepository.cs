using AaramEducation.Core.Entities;

namespace AaramEducation.Core.Interfaces;

public interface IBadgeRepository
{
    Task<IEnumerable<Badge>> GetAllAsync();
    Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId);
    Task CheckAndAwardAsync(int userId);
}
