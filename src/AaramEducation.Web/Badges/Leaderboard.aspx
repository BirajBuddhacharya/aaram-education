<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Leaderboard.aspx.cs" Inherits="AaramEducation.Web.Badges.Leaderboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4" style="max-width:700px">
  <h1 class="mb-4">Leaderboard</h1>

  <asp:Panel ID="pnlUserRank" runat="server" Visible="false" CssClass="alert alert-info">
    Your rank: <strong>#<asp:Literal ID="litUserRank" runat="server" /></strong>
  </asp:Panel>

  <div class="list-group">
    <asp:Repeater ID="rptLeaderboard" runat="server">
      <ItemTemplate>
        <div class='list-group-item d-flex align-items-center gap-3 <%# (bool)Eval("IsCurrentUser") ? "bg-light fw-bold" : "" %>'>
          <span class='<%# (int)Eval("Rank") <= 3 ? "text-warning fw-bold" : "text-muted" %>' style="width:2rem;text-align:center">
            <%# (int)Eval("Rank") == 1 ? "🥇" : (int)Eval("Rank") == 2 ? "🥈" : (int)Eval("Rank") == 3 ? "🥉" : "#" + Eval("Rank") %>
          </span>
          <img src='<%# ResolveUrl("~/wwwroot/img/avatar-default.svg") %>'
               alt="" width="36" height="36" class="rounded-circle" style="object-fit:cover" />
          <span class="flex-grow-1"><%# Server.HtmlEncode((string)Eval("FullName")) %></span>
          <span class="badge bg-warning text-dark"><%# Eval("TotalXpPoints") %> XP</span>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</div>

</asp:Content>
