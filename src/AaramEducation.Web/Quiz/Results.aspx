<%@ Page Title="Quiz Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizResultsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <h1 class="t-h2 mb-2">Results</h1>
    <div class="card p-4 mb-5">
      <div class="d-flex align-items-center gap-4">
        <div class="t-h1" style="font-size:3rem"><asp:Literal ID="litScore" runat="server" />%</div>
        <div>
          <asp:Label ID="lblStatus" runat="server" CssClass="badge fs-6" />
          <p class="t-small text-muted mb-0 mt-1"><asp:Literal ID="litDetails" runat="server" /></p>
        </div>
      </div>
    </div>

    <h2 class="t-h4 mb-3">Question review</h2>
    <asp:Repeater ID="rptResponses" runat="server">
      <ItemTemplate>
        <div class='card p-3 mb-3 border-<%# (bool)Eval("IsCorrect") ? "success" : "danger" %>'>
          <p class="t-body fw-semibold mb-2"><%# System.Web.HttpUtility.HtmlEncode(Eval("Question.QuestionText")) %></p>
          <p class="t-small mb-1">Your answer: <strong><%# System.Web.HttpUtility.HtmlEncode(Eval("SelectedOption") != null ? Eval("SelectedOption.OptionText") : Eval("TextAnswer")) %></strong></p>
          <asp:Label runat="server" Visible='<%# !(bool)Eval("IsCorrect") %>' CssClass="t-small text-success">Correct: <%# System.Web.HttpUtility.HtmlEncode(Eval("Question.CorrectAnswer")) %></asp:Label>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <div class="mt-4 d-flex gap-2">
      <asp:HyperLink ID="lnkRetry" runat="server" CssClass="btn btn-outline-primary">Try again</asp:HyperLink>
      <asp:HyperLink ID="lnkLesson" runat="server" CssClass="btn btn-outline-secondary">Back to lesson</asp:HyperLink>
    </div>
  </div>
</asp:Content>
