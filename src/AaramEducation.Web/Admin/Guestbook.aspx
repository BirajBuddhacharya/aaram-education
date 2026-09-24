<%@ Page Title="Moderate Guestbook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Guestbook.aspx.cs" Inherits="AaramEducation.Web.Admin.GuestbookModerationPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1>Moderate Guestbook</h1>
      <a href="~/Admin/Dashboard.aspx" runat="server" class="btn btn-outline-secondary">Back</a>
    </div>
    <h4>Pending Entries</h4>
    <asp:Repeater ID="rptPending" runat="server" OnItemCommand="Pending_Command">
      <ItemTemplate>
        <div class="card mb-3 p-4">
          <p class="fw-semibold mb-1"><%# System.Web.HttpUtility.HtmlEncode((string)Eval("GuestName")) %>
            <% if (Eval("GuestEmail") != null) { %>
              &lt;<%# System.Web.HttpUtility.HtmlEncode((string)Eval("GuestEmail")) %>&gt;
            <% } %>
          </p>
          <p class="mb-3"><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Message")) %></p>
          <div class="d-flex gap-2">
            <asp:Button runat="server" CommandName="Approve" CommandArgument='<%# Eval("EntryId") %>'
              Text="Approve" CssClass="btn btn-sm btn-success" />
            <asp:Button runat="server" CommandName="Reject" CommandArgument='<%# Eval("EntryId") %>'
              Text="Reject" CssClass="btn btn-sm btn-warning" />
            <asp:Button runat="server" CommandName="Delete" CommandArgument='<%# Eval("EntryId") %>'
              Text="Delete" CssClass="btn btn-sm btn-danger" />
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
    <asp:Label ID="lblNone" runat="server" Visible="false" CssClass="text-muted">No pending entries.</asp:Label>
  </div>
</asp:Content>
