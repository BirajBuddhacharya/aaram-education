<%@ Page Title="Lesson Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LessonForm.aspx.cs" Inherits="AaramEducation.Web.Tutor.LessonForm" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <h1 class="mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:HiddenField ID="hdnLessonId" runat="server" />
    <asp:HiddenField ID="hdnModuleId" runat="server" />

    <h5 class="mb-3">Lesson Info</h5>
    <div class="mb-3">
      <label class="form-label">Title</label>
      <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="200" />
    </div>
    <div class="mb-3">
      <label class="form-label">Description</label>
      <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
    </div>
    <div class="row g-3 mb-4">
      <div class="col-md-6">
        <label class="form-label">Order</label>
        <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
      </div>
      <div class="col-md-6 d-flex align-items-end">
        <div class="form-check mb-2">
          <asp:CheckBox ID="chkFreeSample" runat="server" CssClass="form-check-input" />
          <label class="form-check-label">Free sample</label>
        </div>
      </div>
    </div>

    <h5 class="mb-3">Video</h5>
    <asp:HiddenField ID="hdnVideoId" runat="server" />
    <div class="mb-3">
      <label class="form-label">Video Title</label>
      <asp:TextBox ID="txtVideoTitle" runat="server" CssClass="form-control" MaxLength="200" />
    </div>
    <div class="mb-4">
      <label class="form-label">Video URL</label>
      <asp:TextBox ID="txtVideoUrl" runat="server" CssClass="form-control" />
    </div>

    <h5 class="mb-3">Study Note</h5>
    <asp:HiddenField ID="hdnNoteId" runat="server" />
    <div class="mb-3">
      <label class="form-label">Note Title</label>
      <asp:TextBox ID="txtNoteTitle" runat="server" CssClass="form-control" MaxLength="200" />
    </div>
    <div class="mb-4">
      <label class="form-label">Note Content</label>
      <asp:TextBox ID="txtNoteContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" />
    </div>

    <div class="d-flex gap-2">
      <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-aaram-primary" OnClick="btnSave_Click" />
      <asp:HyperLink ID="lnkCancel" runat="server" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
    </div>
  </div>
</asp:Content>
