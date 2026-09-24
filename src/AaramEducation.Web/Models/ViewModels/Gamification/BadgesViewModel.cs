using AaramEducation.Core.Entities;
namespace AaramEducation.Web.Models.ViewModels.Gamification;
public class BadgeDisplayItem
{
    public Badge Badge { get; set; } = null!;
    public bool Earned { get; set; }
    public DateTime? EarnedAt { get; set; }
}
public class BadgesViewModel
{
    public IEnumerable<BadgeDisplayItem> Badges { get; set; } = [];
    public int EarnedCount { get; set; }
    public int TotalCount { get; set; }
}
