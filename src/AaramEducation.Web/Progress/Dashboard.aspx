<%@ Page Title="My Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Progress.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="container" style="padding-top:40px;padding-bottom:80px">

  <h1 class="t-h1 mb-5">Welcome back, <%= Server.HtmlEncode(AaramEducation.Web.Helpers.AuthHelper.GetCurrentUserFirstName()) %>!</h1>

  <div class="row g-3 mb-5">
    <div class="col-md-4">
      <div class="card-aaram text-center">
        <div class="t-hero" style="font-size:40px;letter-spacing:-.02em;color:var(--aaram-primary)"><asp:Literal ID="litXp" runat="server" /></div>
        <div class="t-small mt-1">XP points</div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card-aaram text-center">
        <div class="t-hero" style="font-size:40px;letter-spacing:-.02em;color:var(--aaram-amber-text)"><asp:Literal ID="litStreak" runat="server" /></div>
        <div class="t-small mt-1">Day streak (longest <asp:Literal ID="litLongestStreak" runat="server" />)</div>
      </div>
    </div>
    <div class="col-md-4">
      <div class="card-aaram text-center">
        <div class="t-hero" style="font-size:40px;letter-spacing:-.02em;color:var(--aaram-primary)"><asp:Literal ID="litBadges" runat="server" /></div>
        <div class="t-small mt-1">Badges earned</div>
      </div>
    </div>
  </div>

  <h2 class="t-h3 mb-3">My courses</h2>
  <asp:Literal ID="litNoCourses" runat="server" />
  <div class="row g-3 mb-5">
    <asp:Repeater ID="rptCourses" runat="server">
      <ItemTemplate>
        <div class="col-md-6 col-lg-4">
          <div class="course-card">
            <div class="t-h4"><%# Server.HtmlEncode((string)Eval("CourseName")) %></div>
            <span class="pill-free align-self-start"><%# (float)Eval("PercentComplete") >= 100f ? "Completed" : ((float)Eval("PercentComplete") > 0f ? "In progress" : "Not started") %></span>
            <div class="progress-bar-aaram green">
              <div class="fill" style='width:<%# Eval("PercentComplete", "{0:F1}") %>%'></div>
            </div>
            <div class="t-xs mb-2"><%# Eval("PercentComplete", "{0:F1}") %>% complete</div>
            <a href='<%# ResolveUrl("~/Courses/Details.aspx?id=" + Eval("CourseId")) %>'
               class="btn-aaram btn-aaram-outline btn-aaram-sm mt-auto">Continue</a>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>

  <div class="row g-4">
    <div class="col-md-6">
      <h2 class="t-h3 mb-3">Recent badges</h2>
      <div class="d-flex flex-column">
        <asp:Repeater ID="rptRecentBadges" runat="server">
          <ItemTemplate>
            <div class="d-flex align-items-center gap-3" style="padding:14px 0;border-top:1px solid var(--aaram-border)">
              <span class="user-avatar" style="width:38px;height:38px;font-size:16px">🏅</span>
              <div>
                <div style="font-weight:700"><%# Server.HtmlEncode((string)Eval("Badge.BadgeName")) %></div>
                <div class="t-xs">+<%# Eval("Badge.XpReward") %> XP · <%# Eval("EarnedAt", "{0:MMM d}") %></div>
              </div>
            </div>
          </ItemTemplate>
        </asp:Repeater>
      </div>
      <a href='<%= ResolveUrl("~/Badges/Index.aspx") %>' class="link-aaram d-inline-block mt-3">View all badges</a>
    </div>

    <div class="col-md-6">
      <h2 class="t-h3 mb-3">This week</h2>
      <table class="table table-sm">
        <thead>
          <tr class="t-xs"><th>Day</th><th>Lessons</th><th>Quizzes</th><th>XP</th></tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptActivity" runat="server">
            <ItemTemplate>
              <tr>
                <td><%# Eval("ActivityDate", "{0:ddd MMM d}") %></td>
                <td><%# Eval("LessonsCompleted") %></td>
                <td><%# Eval("QuizzesPassed") %></td>
                <td><%# Eval("XpEarned") %></td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>

</div>

</asp:Content>
