<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="AaramEducation.Web.Admin.UsersPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1>Users</h1>
      <a href='<%= ResolveUrl("~/Admin/Dashboard.aspx") %>' class="btn btn-outline-secondary btn-sm">← Dashboard</a>
    </div>

    <div class="table-responsive">
      <table class="table table-hover align-middle">
        <thead class="table-light">
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
            <th>XP</th>
            <th>Joined</th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
              <tr>
                <td><%# System.Web.HttpUtility.HtmlEncode(Eval("FirstName") + " " + Eval("LastName")) %></td>
                <td class="text-muted small"><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Email")) %></td>
                <td>
                  <span class='<%# "badge " + ((AaramEducation.Core.Enums.UserRole)Eval("Role") == AaramEducation.Core.Enums.UserRole.Admin ? "bg-danger" : (AaramEducation.Core.Enums.UserRole)Eval("Role") == AaramEducation.Core.Enums.UserRole.Tutor ? "bg-warning text-dark" : "bg-secondary") %>'>
                    <%# Eval("Role") %>
                  </span>
                </td>
                <td><%# Eval("TotalXpPoints") %></td>
                <td class="text-muted small"><%# ((DateTime)Eval("CreatedAt")).ToString("d MMM yyyy") %></td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
