<%@ Page Title="Quiz Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizResultsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container py-4" style="max-width:800px">

  <h1 class="mb-1">Results</h1>

  <div class="text-center my-4">
    <div class="display-1 fw-bold"><asp:Literal ID="litScore" runat="server" />%</div>
    <asp:Label ID="lblStatus" runat="server" CssClass="badge fs-5" />
    <p class="text-muted mt-2"><asp:Literal ID="litDetails" runat="server" /></p>
  </div>

  <h5 class="mt-4 mb-3">Question Breakdown</h5>

  <asp:Repeater ID="rptResponses" runat="server">
    <ItemTemplate>
      <div class='card mb-3 border-<%# (bool)Eval("IsCorrect") ? "success" : "danger" %>'>
        <div class="card-body">
          <div class="d-flex justify-content-between">
            <p class="fw-semibold mb-2"><%# Server.HtmlEncode((string)Eval("Question.QuestionText")) %></p>
            <span class="fs-5"><%# (bool)Eval("IsCorrect") ? "&#10003;" : "&#10007;" %></span>
          </div>
          <p class="mb-1 small">
            <span class="text-muted">Your answer:</span>
            <%# Eval("SelectedOption") != null
                  ? Server.HtmlEncode((string)Eval("SelectedOption.OptionText"))
                  : (string.IsNullOrEmpty((string)Eval("TextResponse"))
                       ? "(no answer)"
                       : Server.HtmlEncode((string)Eval("TextResponse"))) %>
          </p>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>

  <div class="d-flex gap-3 mt-4">
    <asp:HyperLink ID="lnkRetry" runat="server" CssClass="btn btn-outline-primary">Retake Quiz</asp:HyperLink>
    <asp:HyperLink ID="lnkLesson" runat="server" CssClass="btn btn-outline-secondary">Back to Lesson</asp:HyperLink>
  </div>
</div>
</asp:Content>
