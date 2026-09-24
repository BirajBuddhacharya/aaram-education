<%@ Page Title="Course Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseForm.aspx.cs" Inherits="AaramEducation.Web.Tutor.CourseForm" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <h1 class="mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:HiddenField ID="hdnCourseId" runat="server" />
    <div class="mb-3">
      <label class="form-label">Course Name</label>
      <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="200" />
    </div>
    <div class="mb-3">
      <label class="form-label">Description</label>
      <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
    </div>
    <div class="row g-3 mb-3">
      <div class="col-md-6">
        <label class="form-label">Subject</label>
        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" MaxLength="100" />
      </div>
      <div class="col-md-6">
        <label class="form-label">Difficulty</label>
        <asp:DropDownList ID="ddlDifficulty" runat="server" CssClass="form-select">
          <asp:ListItem Value="Beginner">Beginner</asp:ListItem>
          <asp:ListItem Value="Intermediate">Intermediate</asp:ListItem>
          <asp:ListItem Value="Advanced">Advanced</asp:ListItem>
        </asp:DropDownList>
      </div>
    </div>
    <div class="mb-4 form-check">
      <asp:CheckBox ID="chkPublish" runat="server" CssClass="form-check-input" />
      <label class="form-check-label">Publish immediately</label>
    </div>
    <div class="d-flex gap-2">
      <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-aaram-primary" OnClick="btnSave_Click" />
      <a href="~/Tutor/Dashboard.aspx" runat="server" class="btn btn-outline-secondary">Cancel</a>
    </div>
  </div>
</asp:Content>
