<%@ Page Title="Badges" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Badges.Index" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <h1 class="mb-4">Badges</h1>
    <div class="row g-4">
      <asp:Repeater ID="rptBadges" runat="server">
        <ItemTemplate>
          <div class="col-sm-6 col-md-4 col-lg-3">
            <div class='card text-center p-4 <%# (bool)Eval("Earned") ? "" : "opacity-50" %>'>
              <img src='<%# Eval("IconUrl") %>' alt='<%# Eval("BadgeName") %>' class="mx-auto mb-3" style="width:64px;height:64px;object-fit:contain;" />
              <h6 class="mb-1"><%# Eval("BadgeName") %></h6>
              <p class="t-small text-muted mb-2"><%# Eval("Description") %></p>
              <span class="badge <%# (bool)Eval("Earned") ? "bg-success" : "bg-secondary" %>">
                <%# (bool)Eval("Earned") ? "Earned" : "Locked" %>
              </span>
              <% if ((bool)Eval("Earned")) { %>
              <div class="t-small text-muted mt-1"><%# Eval("EarnedAt", "{0:MMM d, yyyy}") %></div>
              <% } %>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
    <div class="mt-4">
      <a href="~/Badges/Leaderboard.aspx" runat="server" class="btn btn-outline-primary">View Leaderboard</a>
    </div>
  </div>
</asp:Content>
