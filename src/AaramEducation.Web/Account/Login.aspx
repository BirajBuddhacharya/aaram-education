<%@ Page Title="Sign in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AaramEducation.Web.Account.LoginPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="auth-wrap">
  <h1 class="t-h2 text-center mb-2">Welcome back</h1>
  <p class="t-small text-center mb-4">Sign in to pick up where you left off.</p>

  <div class="auth-card">
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block" role="alert" Visible="false" />

    <div class="mb-3">
      <label class="form-label" for="<%= txtEmail.ClientID %>">Email</label>
      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" autocomplete="email" placeholder="you@example.com" />
      <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" CssClass="text-danger small" ErrorMessage="Email is required." Display="Dynamic" />
    </div>

    <div class="mb-3">
      <label class="form-label" for="<%= txtPassword.ClientID %>">Password</label>
      <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="current-password" placeholder="At least 8 characters" />
      <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" CssClass="text-danger small" ErrorMessage="Password is required." Display="Dynamic" />
    </div>

    <div class="form-check mb-4">
      <asp:CheckBox ID="chkRemember" runat="server" CssClass="form-check-input" />
      <label class="form-check-label t-small" for="<%= chkRemember.ClientID %>">Remember me for 7 days</label>
    </div>

    <asp:Button ID="btnLogin" runat="server" Text="Sign in" CssClass="btn-aaram btn-aaram-primary btn-aaram-block" OnClick="Login_Click" />
  </div>

  <p class="t-small text-center mt-4 mb-0">
    New to Aaram? <a href='<%= ResolveUrl("~/Account/Register.aspx") %>' class="link-aaram">Create an account</a>
  </p>
</div>

</asp:Content>
