<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AaramEducation.Web.Account.ProfilePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4" style="max-width:640px">
  <h1 class="mb-4">My Profile</h1>

  <asp:Label ID="lblSuccess" runat="server" CssClass="alert alert-success d-block" Visible="false" />
  <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block" role="alert" Visible="false" />

  <div class="text-center mb-4">
    <img src='<%= ResolveUrl("~/wwwroot/img/avatar-default.svg") %>'
         alt="Profile picture" class="rounded-circle" width="96" height="96"
         style="object-fit:cover;border:3px solid var(--aaram-primary)" />
    <div class="mt-2">
      <label for="NewProfilePicture" class="btn btn-sm btn-outline-secondary">Change photo</label>
      <input name="NewProfilePicture" id="NewProfilePicture" type="file"
             accept="image/jpeg,image/png,image/webp" class="visually-hidden" />
    </div>
  </div>

  <div class="row g-3">
    <div class="col-6">
      <label class="form-label" for="<%= txtFirstName.ClientID %>">First name</label>
      <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
    <div class="col-6">
      <label class="form-label" for="<%= txtLastName.ClientID %>">Last name</label>
      <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
    </div>
  </div>

  <div class="mt-3">
    <label class="form-label" for="<%= txtEmail.ClientID %>">Email address</label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" CssClass="text-danger small" ErrorMessage="Required." Display="Dynamic" />
  </div>

  <hr class="my-4" />
  <h5 class="mb-3">Change password <small class="text-muted fs-6">(leave blank to keep current)</small></h5>

  <div class="mt-3">
    <label class="form-label" for="<%= txtNewPassword.ClientID %>">New password</label>
    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
  </div>

  <div class="mt-3">
    <label class="form-label" for="<%= txtConfirmNewPassword.ClientID %>">Confirm new password</label>
    <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="new-password" />
    <asp:CompareValidator runat="server" ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword"
                          CssClass="text-danger small" ErrorMessage="Passwords do not match." Display="Dynamic" />
  </div>

  <div class="mt-4 d-flex gap-2">
    <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="btn btn-primary" OnClick="Save_Click" />
    <a href='<%= ResolveUrl("~/Default.aspx") %>' class="btn btn-outline-secondary">Cancel</a>
  </div>
</div>

</asp:Content>
