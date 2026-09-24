<%@ Page Title="Create account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="AaramEducation.Web.Account.RegisterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="auth-wrap">
    <h1 class="t-h2 text-center mb-2">Create your account</h1>
    <p class="t-small text-center mb-4">Join thousands of learners on Aaram.</p>
    <div class="auth-card">
      <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block mb-3" Visible="false" />
      <div class="row mb-3">
        <div class="col-6">
          <label class="form-label">First name</label>
          <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="First" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
        </div>
        <div class="col-6">
          <label class="form-label">Last name</label>
          <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Last" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
        </div>
      </div>
      <div class="mb-3">
        <label class="form-label">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="you@example.com" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger small" ErrorMessage="Email is required." Display="Dynamic" />
      </div>
      <div class="mb-3">
        <label class="form-label">Password</label>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="At least 8 characters" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" CssClass="text-danger small" ErrorMessage="Password is required." Display="Dynamic" />
      </div>
      <div class="mb-4">
        <label class="form-label">I am a</label>
        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
          <asp:ListItem Value="Student">Student</asp:ListItem>
          <asp:ListItem Value="Tutor">Tutor</asp:ListItem>
        </asp:DropDownList>
      </div>
      <asp:Button ID="btnRegister" runat="server" Text="Create account" CssClass="btn-aaram btn-aaram-primary btn-aaram-block" OnClick="Register_Click" />
    </div>
    <p class="t-small text-center mt-4 mb-0">
      Already have an account? <a href="~/Account/Login.aspx" runat="server" class="link-aaram">Sign in</a>
    </p>
  </div>
</asp:Content>
