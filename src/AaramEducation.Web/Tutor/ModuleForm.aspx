<%@ Page Title="Module Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModuleForm.aspx.cs" Inherits="AaramEducation.Web.Tutor.ModuleForm" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:600px">
    <h1 class="mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:HiddenField ID="hdnModuleId" runat="server" />
    <asp:HiddenField ID="hdnCourseId" runat="server" />
    <div class="mb-3">
      <label class="form-label">Module Name</label>
      <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="200" />
    </div>
    <div class="mb-3">
      <label class="form-label">Description</label>
      <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
    </div>
    <div class="mb-4">
      <label class="form-label">Order</label>
      <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
    </div>
    <div class="d-flex gap-2">
      <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-aaram-primary" OnClick="btnSave_Click" />
      <asp:HyperLink ID="lnkCancel" runat="server" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
    </div>
  </div>
</asp:Content>
