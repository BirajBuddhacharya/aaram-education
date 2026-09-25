<%@ Page Title="Create account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AaramEducation.Web.Account.RegisterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="auth-card">
  <h1 class="auth-title">Create your account</h1>
  <p class="auth-sub">Start learning at your own pace.</p>

  <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block" role="alert" Visible="false" />

  <div class="row g-3">
    <div class="col-6">
      <label class="form-label" for="<%= txtFirstName.ClientID %>">First name</label>
      <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" autocomplete="given-name" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
    <div class="col-6">
      <label class="form-label" for="<%= txtLastName.ClientID %>">Last name</label>
      <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" autocomplete="family-name" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
  </div>

  <div class="mt-3">
    <label class="form-label" for="<%= txtEmail.ClientID %>">Email address</label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" autocomplete="email" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger small" ErrorMessage="Email is required." Display="Dynamic" />
  </div>

  <div class="mt-3">
    <label class="form-label" for="<%= txtPassword.ClientID %>">Password</label>
    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger small" ErrorMessage="Password is required." Display="Dynamic" />
    <div id="pw-strength" class="mt-1"></div>
  </div>

  <div class="mt-3">
    <label class="form-label" for="<%= txtConfirmPassword.ClientID %>">Confirm password</label>
    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
    <asp:CompareValidator runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                          CssClass="text-danger small" ErrorMessage="Passwords do not match." Display="Dynamic" />
  </div>

  <div class="mt-3">
    <label class="form-label">I am joining as a</label>
    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
      <asp:ListItem Value="Student">&#127891; Student</asp:ListItem>
      <asp:ListItem Value="Tutor">&#127979; Tutor</asp:ListItem>
    </asp:DropDownList>
  </div>

  <asp:Button ID="btnRegister" runat="server" Text="Create account" CssClass="btn btn-primary w-100 mt-4" OnClick="Register_Click" />

  <p class="text-center mt-3 text-muted small">
    Already have an account? <a href='<%= ResolveUrl("~/Account/Login.aspx") %>'>Sign in</a>
  </p>
</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
  <script src='<%= ResolveUrl("~/wwwroot/js/validation.js") %>'></script>
</asp:Content>
