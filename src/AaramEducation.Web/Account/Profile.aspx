<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AaramEducation.Web.Account.ProfilePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:600px">
    <h1 class="t-h2 mb-4">Your Profile</h1>
    <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success py-2 d-block mb-3" Visible="false" />
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block mb-3" Visible="false" />
    <div class="mb-3">
      <label class="form-label">First name</label>
      <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
    <div class="mb-3">
      <label class="form-label">Last name</label>
      <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
    <div class="mb-3">
      <label class="form-label">Email</label>
      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
    <div class="mb-3">
      <label class="form-label">New password <span class="t-small text-muted">(leave blank to keep current)</span></label>
      <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" />
    </div>
    <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="btn-aaram btn-aaram-primary" OnClick="Save_Click" />
  </div>
</asp:Content>
