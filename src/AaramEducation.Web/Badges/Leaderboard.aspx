<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="AaramEducation.Web.Badges.Leaderboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1>Leaderboard</h1>
      <a href="~/Badges/Index.aspx" runat="server" class="btn btn-outline-primary">My Badges</a>
    </div>
    <div class="card p-0">
      <table class="table table-hover mb-0">
        <thead>
          <tr><th>#</th><th>Student</th><th>XP</th><th>Streak</th></tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptLeaderboard" runat="server">
            <ItemTemplate>
              <tr class='<%# (bool)Eval("IsCurrentUser") ? "table-primary fw-bold" : "" %>'>
                <td><%# Eval("Rank") %></td>
                <td><%# Eval("FullName") %></td>
                <td><%# Eval("TotalXpPoints") %></td>
                <td><%# Eval("CurrentStreakDays") %> days</td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
