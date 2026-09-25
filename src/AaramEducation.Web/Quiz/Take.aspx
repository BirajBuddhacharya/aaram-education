<%@ Page Title="Take Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Take.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizTakePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="quiz-timer-bar sticky-top py-2 bg-light border-bottom text-end px-3">
  Time: <span id="quiz-timer" class="fw-bold">00:00</span>
</div>

<div class="container py-4" style="max-width:800px">
  <h1 class="mb-4"><asp:Literal ID="litQuizTitle" runat="server" /></h1>

  <asp:HiddenField ID="hfAttemptId" runat="server" />
  <asp:HiddenField ID="hfQuestionIndex" runat="server" Value="0" />
  <asp:HiddenField ID="hfQuizId" runat="server" />

  <asp:Panel ID="pnlQuestion" runat="server">
    <div class="card mb-4">
      <div class="card-body">
        <p class="text-muted small mb-2">
          Question <asp:Literal ID="litQNum" runat="server" /> of <asp:Literal ID="litQTotal" runat="server" />
        </p>
        <p class="fw-semibold mb-3"><asp:Literal ID="litQuestion" runat="server" /></p>

        <asp:RadioButtonList ID="rblOptions" runat="server"
                             RepeatLayout="Flow" RepeatDirection="Vertical"
                             CssClass="quiz-options" />

        <asp:TextBox ID="txtShortAnswer" runat="server" CssClass="form-control"
                     TextMode="MultiLine" Rows="3" Visible="false"
                     placeholder="Your answer..." />
      </div>
    </div>

    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-primary btn-lg" OnClick="Next_Click" />
  </asp:Panel>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
  <script src='<%= ResolveUrl("~/wwwroot/js/quiz-timer.js") %>'></script>
</asp:Content>
