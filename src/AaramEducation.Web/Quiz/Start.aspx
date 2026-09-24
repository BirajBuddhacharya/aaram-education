<%@ Page Title="Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="AaramEducation.Web.Quiz.QuizStartPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:700px">
    <asp:Literal ID="litMessage" runat="server" />
    <div class="card p-5 mb-4">
      <h1 class="mb-2"><asp:Literal ID="litTitle" runat="server" /></h1>
      <p><asp:Literal ID="litDescription" runat="server" /></p>
      <div class="row g-3 mb-4">
        <div class="col-auto">
          <span class="badge bg-primary fs-6">Passing: <asp:Literal ID="litPassing" runat="server" />%</span>
        </div>
        <div class="col-auto">
          <span class="badge bg-secondary fs-6">Max attempts: <asp:Literal ID="litMaxAttempts" runat="server" /></span>
        </div>
        <div class="col-auto">
          <span class="badge bg-info fs-6">Attempts used: <asp:Literal ID="litAttemptsUsed" runat="server" /></span>
        </div>
      </div>
      <asp:Button ID="btnStart" runat="server" Text="Start Quiz" CssClass="btn btn-aaram-primary btn-lg" OnClick="btnStart_Click" />
    </div>

    <asp:Panel ID="pnlHistory" runat="server" Visible="false">
      <h4 class="mb-3">Attempt History</h4>
      <asp:Repeater ID="rptAttempts" runat="server">
        <ItemTemplate>
          <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
            <span><%# Eval("AttemptDate", "{0:MMM d, yyyy HH:mm}") %></span>
            <span class="badge <%# (int)Eval("ScoreAchieved") >= (int)Eval("PassingScore") ? "bg-success" : "bg-danger" %>">
              <%# Eval("ScoreAchieved") %>%
            </span>
            <a href='<%# "~/Quiz/Results.aspx?id=" + Eval("AttemptId") %>' runat="server" class="btn btn-sm btn-outline-secondary">View</a>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </asp:Panel>

    <div class="mt-4">
      <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-link">← Back to Lesson</asp:HyperLink>
    </div>
  </div>
</asp:Content>
