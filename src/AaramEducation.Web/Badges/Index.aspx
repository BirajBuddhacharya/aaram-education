<%@ Page Title="Badges" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Badges.Index" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4">
  <div class="d-flex align-items-center justify-content-between mb-4">
    <h1>Badges</h1>
    <span class="badge bg-primary fs-6"><asp:Literal ID="litEarnedCount" runat="server" /> / <asp:Literal ID="litTotalCount" runat="server" /> earned</span>
  </div>

  <div class="row g-3">
    <asp:Repeater ID="rptBadges" runat="server">
      <ItemTemplate>
        <div class="col-6 col-md-4 col-lg-3">
          <div class='card text-center h-100 <%# (bool)Eval("Earned") ? "" : "opacity-50" %>'>
            <div class="card-body">
              <div class="badge-icon mb-2" style="font-size:3rem">
                <%# string.IsNullOrWhiteSpace((string)Eval("IconUrl"))
                      ? "<span>&#x1F3C5;</span>"
                      : "<img src='" + Server.HtmlEncode((string)Eval("IconUrl")) + "' alt='" + Server.HtmlEncode((string)Eval("BadgeName")) + "' width='64' height='64' />" %>
              </div>
              <h6 class="card-title"><%# Server.HtmlEncode((string)Eval("BadgeName")) %></h6>
              <p class="card-text text-muted small"><%# Server.HtmlEncode((string)Eval("Description")) %></p>
              <span class="badge bg-warning text-dark">+<%# Eval("XpReward") %> XP</span>
              <%# (bool)Eval("Earned")
                    ? (Eval("EarnedAt") != null
                        ? "<p class='mt-2 text-success small'>Earned " + Convert.ToDateTime(Eval("EarnedAt")).ToString("d MMM yyyy") + "</p>"
                        : "")
                    : "<p class='mt-2 text-muted small'>Not yet earned</p>" %>
            </div>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</div>

</asp:Content>
