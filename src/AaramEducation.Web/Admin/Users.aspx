<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="AaramEducation.Web.Admin.UsersPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1>Users</h1>
      <a href="~/Admin/Dashboard.aspx" runat="server" class="btn btn-outline-secondary">Back</a>
    </div>
    <div class="table-responsive">
      <table class="table">
        <thead><tr><th>Name</th><th>Email</th><th>Role</th><th>XP</th><th>Streak</th><th>Joined</th></tr></thead>
        <tbody>
          <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
              <tr>
                <td><%# System.Web.HttpUtility.HtmlEncode(Eval("FirstName") + " " + Eval("LastName")) %></td>
                <td><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Email")) %></td>
                <td><span class="badge bg-secondary"><%# Eval("Role") %></span></td>
                <td><%# Eval("TotalXpPoints") %></td>
                <td><%# Eval("CurrentStreakDays") %></td>
                <td><%# ((DateTime)Eval("CreatedAt")).ToString("dd MMM yyyy") %></td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
