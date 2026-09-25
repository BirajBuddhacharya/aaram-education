<%@ Page Title="Guestbook" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Guestbook.GuestbookIndexPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4">
  <div class="row g-4">

    <div class="col-lg-7">
      <h1 class="mb-1">Guestbook</h1>
      <p class="text-muted mb-4"><asp:Literal ID="litCount" runat="server" /> message(s) from our community.</p>

      <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success d-block" Visible="false" />

      <asp:Repeater ID="rptEntries" runat="server">
        <ItemTemplate>
          <div class="card mb-3">
            <div class="card-body">
              <div class="d-flex justify-content-between align-items-start">
                <h6 class="card-title mb-1"><%# Server.HtmlEncode((string)Eval("GuestName")) %></h6>
                <small class="text-muted"><%# Eval("SubmittedAt", "{0:d MMM yyyy}") %></small>
              </div>
              <p class="card-text"><%# Server.HtmlEncode((string)Eval("Message")) %></p>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>

      <% if (TotalPages > 1) { %>
      <nav aria-label="Guestbook pages">
        <ul class="pagination">
          <% for (int p = 1; p <= TotalPages; p++) { %>
          <li class="page-item <%= p == CurrentPage ? "active" : "" %>">
            <a class="page-link" href='<%= ResolveUrl("~/Guestbook/Index.aspx?page=" + p) %>'><%= p %></a>
          </li>
          <% } %>
        </ul>
      </nav>
      <% } %>
    </div>

    <div class="col-lg-5">
      <div class="card sticky-top" style="top:80px">
        <div class="card-header fw-semibold">Leave a Message</div>
        <div class="card-body">
          <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger small d-block" Visible="false" />

          <div class="mb-3">
            <label class="form-label">Your name</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" CssClass="text-danger small" ErrorMessage="Name is required." Display="Dynamic" />
          </div>
          <div class="mb-3">
            <label class="form-label">Email <small class="text-muted">(optional)</small></label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
          </div>
          <div class="mb-3">
            <label class="form-label">Message</label>
            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"
                         MaxLength="1000" placeholder="Share your experience..." />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMessage" CssClass="text-danger small" ErrorMessage="Message is required." Display="Dynamic" />
          </div>
          <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary w-100" OnClick="Submit_Click" />

          <p class="text-muted small mt-2 mb-0">Messages are reviewed before appearing.</p>
        </div>
      </div>
    </div>

  </div>
</div>

</asp:Content>
