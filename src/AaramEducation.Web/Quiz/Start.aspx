<%@ Page Title="Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizStartPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="container py-4" style="max-width:720px">

  <h1 class="mb-1"><asp:Literal ID="litTitle" runat="server" /></h1>
  <p class="text-muted"><asp:Literal ID="litDescription" runat="server" /></p>

  <div class="d-flex gap-3 mb-4">
    <span class="badge bg-secondary fs-6">Passing: <asp:Literal ID="litPassing" runat="server" />%</span>
    <span class="badge bg-secondary fs-6">Attempts: <asp:Literal ID="litAttemptsUsed" runat="server" /> / <asp:Literal ID="litMaxAttempts" runat="server" /></span>
  </div>

  <asp:Literal ID="litMessage" runat="server" />

  <div class="mb-4">
    <asp:Button ID="btnStart" runat="server" Text="Begin Quiz" CssClass="btn btn-primary" OnClick="btnStart_Click" />
  </div>

  <asp:Panel ID="pnlHistory" runat="server" Visible="false">
    <h5 class="mt-4">Past Attempts</h5>
    <table class="table table-sm">
      <thead>
        <tr><th>#</th><th>Date</th><th>Score</th><th>Result</th><th></th></tr>
      </thead>
      <tbody>
        <asp:Repeater ID="rptAttempts" runat="server">
          <ItemTemplate>
            <tr>
              <td><%# Container.ItemIndex + 1 %></td>
              <td><%# Eval("AttemptDate", "{0:dd MMM yyyy HH:mm}") %></td>
              <td><%# Eval("ScoreAchieved") %>%</td>
              <td>
                <span class='badge <%# (int)Eval("ScoreAchieved") >= (int)Eval("Quiz.PassingScore") ? "bg-success" : "bg-danger" %>'>
                  <%# (int)Eval("ScoreAchieved") >= (int)Eval("Quiz.PassingScore") ? "Pass" : "Fail" %>
                </span>
              </td>
              <td>
                <a href='<%# ResolveUrl("~/Quiz/Results.aspx?attemptId=" + Eval("AttemptId")) %>'
                   class="btn btn-sm btn-outline-secondary">View</a>
              </td>
            </tr>
          </ItemTemplate>
        </asp:Repeater>
      </tbody>
    </table>
  </asp:Panel>

  <div class="mt-4">
    <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-link">&larr; Back to lesson</asp:HyperLink>
  </div>
</div>
</asp:Content>
