namespace AaramEducation.Core.Entities;

public class Badge
{
    public int BadgeId { get; set; }
    public string BadgeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public int XpReward { get; set; }
    public string TargetType { get; set; } = string.Empty;
    public int TargetValue { get; set; }

    public ICollection<UserBadge> UserBadges { get; set; } = [];
}
