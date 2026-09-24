using System;
using System.Collections.Generic;
using System.Linq;
using AaramEducation.Core.Entities;
using AaramEducation.Infrastructure.Data;
using AaramEducation.Infrastructure.Repositories;
using AaramEducation.Web.App_Code;

namespace AaramEducation.Web.Badges
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RequireAuth();
            if (!IsPostBack) LoadBadges();
        }

        private void LoadBadges()
        {
            using (var db = new ApplicationDbContext())
            {
                var repo = new BadgeRepository(db);
                var allBadges = repo.GetAllAsync().Result.ToList();
                var userBadges = repo.GetUserBadgesAsync(CurrentUserId!.Value).Result
                    .ToDictionary(ub => ub.BadgeId, ub => ub.EarnedAt);

                var items = allBadges.Select(b => new
                {
                    b.BadgeId,
                    b.BadgeName,
                    b.Description,
                    b.IconUrl,
                    b.XpReward,
                    Earned = userBadges.ContainsKey(b.BadgeId),
                    EarnedAt = userBadges.ContainsKey(b.BadgeId) ? userBadges[b.BadgeId] : (DateTime?)null
                }).OrderByDescending(x => x.Earned).ThenBy(x => x.BadgeName).ToList();

                rptBadges.DataSource = items;
                rptBadges.DataBind();
            }
        }
    }
}
