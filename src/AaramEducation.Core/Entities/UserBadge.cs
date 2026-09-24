using System;

namespace AaramEducation.Core.Entities
{
    public class UserBadge
    {
        public int UserBadgeId { get; set; }
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public DateTime EarnedAt { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Badge Badge { get; set; } = null!;
    }
}
