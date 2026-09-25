<%@ Page Title="Lesson Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LessonForm.aspx.cs" Inherits="AaramEducation.Web.Tutor.LessonForm" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4" style="max-width:680px">
    <h1 class="mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>

    <asp:Literal ID="litMessage" runat="server" />
    <asp:HiddenField ID="hdnLessonId" runat="server" />
    <asp:HiddenField ID="hdnModuleId" runat="server" />

    <div class="mb-3">
      <label class="form-label">Lesson title</label>
      <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="200" />
    </div>

    <div class="mb-3">
      <label class="form-label">Description <span class="text-muted">(optional)</span></label>
      <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
    </div>

    <div class="row g-3 mb-3">
      <div class="col-4">
        <label class="form-label">Order</label>
        <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" TextMode="Number" min="1" Text="1" />
      </div>
      <div class="col-8 d-flex align-items-end">
        <div class="form-check mb-2">
          <asp:CheckBox ID="chkFreeSample" runat="server" CssClass="form-check-input" />
          <label class="form-check-label">Free sample (visible without enrollment)</label>
        </div>
      </div>
    </div>

    <hr />
    <h5 class="mb-3">Video</h5>

    <asp:HiddenField ID="hdnVideoId" runat="server" />

    <div class="mb-3">
      <label class="form-label">Video URL</label>
      <asp:TextBox ID="txtVideoUrl" runat="server" CssClass="form-control" placeholder="https://youtube.com/... or /uploads/videos/file.mp4" />
      <div class="form-text">YouTube URLs are embedded; local paths served directly.</div>
    </div>

    <div class="row g-3 mb-3">
      <div class="col-8">
        <label class="form-label">Video title <span class="text-muted">(optional)</span></label>
        <asp:TextBox ID="txtVideoTitle" runat="server" CssClass="form-control" MaxLength="200" />
      </div>
      <div class="col-4">
        <label class="form-label">Duration (s)</label>
        <input class="form-control" type="number" min="0" />
      </div>
    </div>

    <hr />
    <h5 class="mb-3">Study Note</h5>

    <asp:HiddenField ID="hdnNoteId" runat="server" />

    <div class="mb-3">
      <label class="form-label">Note title</label>
      <asp:TextBox ID="txtNoteTitle" runat="server" CssClass="form-control" MaxLength="200" />
    </div>

    <div class="mb-3">
      <label class="form-label">Note content <span class="text-muted">(optional)</span></label>
      <asp:TextBox ID="txtNoteContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
    </div>

    <div class="mb-4">
      <label class="form-label">Attach file <span class="text-muted">(optional — PDF, DOCX, etc.)</span></label>
      <input type="file" class="form-control" />
    </div>

    <div class="d-flex gap-2">
      <asp:Button ID="btnSave" runat="server" Text="Save changes" CssClass="btn btn-primary" OnClick="btnSave_Click" />
      <asp:HyperLink ID="lnkCancel" runat="server" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
    </div>
  </div>
</asp:Content>
