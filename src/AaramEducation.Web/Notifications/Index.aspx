<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Notifications.NotificationsIndexPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4" style="max-width:680px">
  <div class="d-flex justify-content-between align-items-center mb-4">
    <h1>Notifications</h1>
    <asp:Button ID="btnMarkAll" runat="server" Text="Mark all read" CssClass="btn btn-sm btn-outline-secondary" OnClick="MarkAllRead_Click" />
  </div>

  <div class="list-group">
    <asp:Repeater ID="rptNotifications" runat="server">
      <ItemTemplate>
        <div class='list-group-item <%# !(bool)Eval("IsRead") ? "list-group-item-light fw-semibold" : "" %>'>
          <div class="d-flex justify-content-between">
            <span><%# Server.HtmlEncode((string)Eval("Title")) %></span>
            <small class="text-muted"><%# Eval("CreatedAt", "{0:d MMM yyyy HH:mm}") %></small>
          </div>
          <p class="mb-0 fw-normal"><%# Server.HtmlEncode((string)Eval("Message")) %></p>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</div>

</asp:Content>
