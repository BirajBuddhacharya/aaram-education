using System;
using System.Linq;
using AaramEducation.Core.Enums;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Web.Helpers;

namespace AaramEducation.Web.Badges
{
    public partial class Leaderboard : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadLeaderboard();
        }

        private void LoadLeaderboard()
        {
            using (var db = new ApplicationDbContext())
            {
                var top = db.Users
                    .Where(u => u.Role == UserRole.Student)
                    .OrderByDescending(u => u.TotalXpPoints)
                    .Take(50)
                    .ToList();

                int rank = 1;
                var items = top.Select(u => new
                {
                    Rank = rank++,
                    FullName = u.FirstName + " " + u.LastName,
                    u.TotalXpPoints,
                    u.CurrentStreakDays,
                    IsCurrentUser = u.UserId == CurrentUserId
                }).ToList();

                rptLeaderboard.DataSource = items;
                rptLeaderboard.DataBind();
            }
        }
    }
}
