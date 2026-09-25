<%@ Page Title="Moderate Guestbook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Guestbook.aspx.cs" Inherits="AaramEducation.Web.Admin.GuestbookModerationPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4" style="max-width:800px">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1>Pending Guestbook Entries</h1>
      <a href='<%= ResolveUrl("~/Admin/Dashboard.aspx") %>' class="btn btn-outline-secondary btn-sm">← Dashboard</a>
    </div>

    <asp:Label ID="lblNone" runat="server" Visible="false" CssClass="alert alert-success d-block">No pending entries — all clear!</asp:Label>

    <asp:Repeater ID="rptPending" runat="server" OnItemCommand="Pending_Command">
      <ItemTemplate>
        <div class="card mb-3">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start mb-2">
              <div>
                <strong><%# System.Web.HttpUtility.HtmlEncode((string)Eval("GuestName")) %></strong>
                <%# string.IsNullOrWhiteSpace((string)Eval("GuestEmail")) ? "" : "<span class=\"text-muted small ms-2\">" + System.Web.HttpUtility.HtmlEncode((string)Eval("GuestEmail")) + "</span>" %>
              </div>
              <span class="text-muted small"><%# ((DateTime)Eval("SubmittedAt")).ToString("d MMM yyyy HH:mm") %></span>
            </div>
            <p class="mb-3"><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Message")) %></p>
            <div class="d-flex gap-2">
              <asp:Button runat="server" CommandName="Approve" CommandArgument='<%# Eval("EntryId") %>'
                Text="Approve" CssClass="btn btn-sm btn-success" />
              <asp:Button runat="server" CommandName="Reject" CommandArgument='<%# Eval("EntryId") %>'
                Text="Reject" CssClass="btn btn-sm btn-warning" />
              <asp:Button runat="server" CommandName="Delete" CommandArgument='<%# Eval("EntryId") %>'
                Text="Delete" CssClass="btn btn-sm btn-outline-danger"
                OnClientClick="return confirm('Permanently delete this entry?');" />
            </div>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</asp:Content>
