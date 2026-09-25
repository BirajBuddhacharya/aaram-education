<%@ Page Title="Module Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModuleForm.aspx.cs" Inherits="AaramEducation.Web.Tutor.ModuleForm" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4" style="max-width:560px">
    <h1 class="mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>

    <asp:Literal ID="litMessage" runat="server" />
    <asp:HiddenField ID="hdnModuleId" runat="server" />
    <asp:HiddenField ID="hdnCourseId" runat="server" />

    <div class="mb-3">
      <label class="form-label">Module name</label>
      <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="200" />
    </div>

    <div class="mb-3">
      <label class="form-label">Description <span class="text-muted">(optional)</span></label>
      <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
    </div>

    <div class="mb-4" style="max-width:120px">
      <label class="form-label">Order</label>
      <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" TextMode="Number" min="1" Text="1" />
    </div>

    <div class="d-flex gap-2">
      <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
      <asp:HyperLink ID="lnkCancel" runat="server" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
    </div>
  </div>
</asp:Content>
