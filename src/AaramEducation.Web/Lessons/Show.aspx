<%@ Page Title="Lesson" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="AaramEducation.Web.Lessons.Show" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <asp:Literal ID="litBreadcrumb" runat="server" />
    <h1 class="mb-4"><asp:Literal ID="litTitle" runat="server" /></h1>
    <asp:Literal ID="litMessage" runat="server" />

    <%-- Video section --%>
    <asp:Panel ID="pnlVideo" runat="server" CssClass="mb-5">
      <div class="ratio ratio-16x9 rounded overflow-hidden mb-3" style="max-width:800px">
        <asp:Literal ID="litVideo" runat="server" />
      </div>
      <h5><asp:Literal ID="litVideoTitle" runat="server" /></h5>
    </asp:Panel>

    <%-- Study note section --%>
    <asp:Panel ID="pnlNote" runat="server" CssClass="card p-4 mb-5">
      <h5 class="mb-3"><asp:Literal ID="litNoteTitle" runat="server" /></h5>
      <div class="prose"><asp:Literal ID="litNoteContent" runat="server" /></div>
    </asp:Panel>

    <%-- Progress buttons --%>
    <asp:Panel ID="pnlProgress" runat="server" CssClass="d-flex gap-3 mb-5 align-items-center">
      <asp:Button ID="btnComplete" runat="server" Text="Mark Complete" CssClass="btn btn-success" OnClick="btnComplete_Click" />
      <asp:Literal ID="litProgressStatus" runat="server" />
    </asp:Panel>

    <%-- Quizzes --%>
    <asp:Panel ID="pnlQuizzes" runat="server">
      <h4 class="mb-3">Quizzes</h4>
      <asp:Repeater ID="rptQuizzes" runat="server">
        <ItemTemplate>
          <div class="card p-3 mb-3 d-flex flex-row justify-content-between align-items-center">
            <div>
              <strong><%# Eval("QuizTitle") %></strong>
              <div class="t-small text-muted">Passing: <%# Eval("PassingScore") %>% &middot; Max attempts: <%# Eval("MaxAttempts") %></div>
            </div>
            <a href='<%# "~/Quiz/Start.aspx?id=" + Eval("QuizId") %>' runat="server" class="btn btn-outline-primary btn-sm">Take Quiz</a>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </asp:Panel>

    <div class="d-flex gap-3 mt-4">
      <asp:HyperLink ID="lnkPrev" runat="server" CssClass="btn btn-outline-secondary" Visible="false">← Previous</asp:HyperLink>
      <asp:HyperLink ID="lnkNext" runat="server" CssClass="btn btn-outline-primary" Visible="false">Next →</asp:HyperLink>
      <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn btn-link">Back to Course</asp:HyperLink>
    </div>
  </div>
</asp:Content>
