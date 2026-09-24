<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Notifications.NotificationsIndexPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1 class="t-h2 mb-0">Notifications</h1>
      <asp:Button ID="btnMarkAll" runat="server" Text="Mark all read" CssClass="btn btn-sm btn-outline-secondary" OnClick="MarkAllRead_Click" />
    </div>
    <asp:Repeater ID="rptNotifications" runat="server">
      <ItemTemplate>
        <div class='card p-3 mb-2 <%# !(bool)Eval("IsRead") ? "border-primary" : "" %>'>
          <div class="d-flex justify-content-between">
            <strong><%# System.Web.HttpUtility.HtmlEncode(Eval("Title")) %></strong>
            <span class="t-small text-muted"><%# ((DateTime)Eval("CreatedAt")).ToString("MMM dd") %></span>
          </div>
          <p class="t-small mb-0"><%# System.Web.HttpUtility.HtmlEncode(Eval("Message")) %></p>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</asp:Content>
