<%@ Page Title="Take Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Take.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizTakePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1 class="t-h3 mb-0"><asp:Literal ID="litQuizTitle" runat="server" /></h1>
      <span class="t-small text-muted">Q <asp:Literal ID="litQNum" runat="server" /> of <asp:Literal ID="litQTotal" runat="server" /></span>
    </div>
    <asp:HiddenField ID="hfAttemptId" runat="server" />
    <asp:HiddenField ID="hfQuestionIndex" runat="server" Value="0" />
    <asp:HiddenField ID="hfQuizId" runat="server" />

    <asp:Panel ID="pnlQuestion" runat="server">
      <div class="card p-4 mb-4">
        <p class="t-body fw-semibold mb-3"><asp:Literal ID="litQuestion" runat="server" /></p>
        <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="list-unstyled" />
        <asp:TextBox ID="txtShortAnswer" runat="server" CssClass="form-control mt-2" Visible="false" placeholder="Your answer..." />
      </div>
      <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-aaram-primary" OnClick="Next_Click" />
    </asp:Panel>
  </div>
</asp:Content>
