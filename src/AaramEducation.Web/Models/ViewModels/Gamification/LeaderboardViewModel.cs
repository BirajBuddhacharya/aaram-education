using AaramEducation.Core.Entities;
namespace AaramEducation.Web.Models.ViewModels.Gamification;
public class LeaderboardEntry
{
    public int Rank { get; set; }
    public User User { get; set; } = null!;
    public int XpPoints { get; set; }
}
public class LeaderboardViewModel
{
    public IEnumerable<LeaderboardEntry> Entries { get; set; } = [];
    public int? CurrentUserRank { get; set; }
}
