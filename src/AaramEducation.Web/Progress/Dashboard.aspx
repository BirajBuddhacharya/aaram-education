<%@ Page Title="My Progress" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Progress.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <h1 class="mb-4">My Learning Dashboard</h1>

    <div class="row g-4 mb-5">
      <div class="col-sm-4">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-primary"><asp:Literal ID="litXp" runat="server" /></div>
          <div class="t-small mt-1">Total XP</div>
        </div>
      </div>
      <div class="col-sm-4">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-success"><asp:Literal ID="litStreak" runat="server" /></div>
          <div class="t-small mt-1">Day Streak</div>
        </div>
      </div>
      <div class="col-sm-4">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-warning"><asp:Literal ID="litBadges" runat="server" /></div>
          <div class="t-small mt-1">Badges Earned</div>
        </div>
      </div>
    </div>

    <h4 class="mb-3">Course Progress</h4>
    <asp:Literal ID="litNoCourses" runat="server" />
    <asp:Repeater ID="rptCourses" runat="server">
      <ItemTemplate>
        <div class="card p-4 mb-3">
          <div class="d-flex justify-content-between align-items-start mb-2">
            <div>
              <h5 class="mb-1"><%# Eval("CourseName") %></h5>
              <span class="badge bg-secondary"><%# Eval("Subject") %></span>
            </div>
            <span class="t-small text-muted"><%# Eval("PercentComplete", "{0:F0}") %>%</span>
          </div>
          <div class="progress mb-2" style="height:8px">
            <div class="progress-bar" style='width:<%# Eval("PercentComplete", "{0:F0}") %>%'></div>
          </div>
          <div class="t-small text-muted mb-2"><%# Eval("LessonsCompleted") %> lessons completed</div>
          <a href='<%# "~/Courses/Details.aspx?id=" + Eval("CourseId") %>' runat="server" class="btn btn-sm btn-outline-primary">Continue</a>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <div class="mt-4 d-flex gap-3">
      <a href="~/Badges/Index.aspx" runat="server" class="btn btn-outline-primary">My Badges</a>
      <a href="~/Badges/Leaderboard.aspx" runat="server" class="btn btn-outline-secondary">Leaderboard</a>
    </div>
  </div>
</asp:Content>
