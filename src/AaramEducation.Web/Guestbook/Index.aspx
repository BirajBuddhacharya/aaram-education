<%@ Page Title="Guestbook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Guestbook.GuestbookIndexPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <h1 class="t-h2 mb-4">Guestbook</h1>
    <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success py-2 d-block mb-4" Visible="false" />

    <div class="card p-4 mb-5">
      <h2 class="t-h4 mb-3">Leave a message</h2>
      <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block mb-3" Visible="false" />
      <div class="mb-3">
        <label class="form-label">Your name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" CssClass="text-danger small" ErrorMessage="Name is required." Display="Dynamic" />
      </div>
      <div class="mb-3">
        <label class="form-label">Email <span class="t-small text-muted">(optional)</span></label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
      </div>
      <div class="mb-3">
        <label class="form-label">Message</label>
        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMessage" CssClass="text-danger small" ErrorMessage="Message is required." Display="Dynamic" />
      </div>
      <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-aaram btn-aaram-primary" OnClick="Submit_Click" />
    </div>

    <h2 class="t-h4 mb-3">Messages <span class="text-muted t-small">(<asp:Literal ID="litCount" runat="server" /> total)</span></h2>
    <asp:Repeater ID="rptEntries" runat="server">
      <ItemTemplate>
        <div class="card p-3 mb-3">
          <div class="d-flex justify-content-between mb-1">
            <strong><%# System.Web.HttpUtility.HtmlEncode(Eval("GuestName")) %></strong>
            <span class="t-small text-muted"><%# ((DateTime)Eval("SubmittedAt")).ToString("MMM dd, yyyy") %></span>
          </div>
          <p class="t-body mb-0"><%# System.Web.HttpUtility.HtmlEncode(Eval("Message")) %></p>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <div class="d-flex gap-2 mt-3">
      <% if (CurrentPage > 1) { %>
      <a href='<%= "~/Guestbook/Index.aspx?page=" + (CurrentPage - 1) %>' class="btn btn-sm btn-outline-secondary">Previous</a>
      <% } %>
      <% if (CurrentPage < TotalPages) { %>
      <a href='<%= "~/Guestbook/Index.aspx?page=" + (CurrentPage + 1) %>' class="btn btn-sm btn-outline-secondary">Next</a>
      <% } %>
    </div>
  </div>
</asp:Content>
