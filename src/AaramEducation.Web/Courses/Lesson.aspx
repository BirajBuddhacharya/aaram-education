<%@ Page Title="Lesson" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Lesson.aspx.cs" Inherits="AaramEducation.Web.Courses.LessonPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="alert alert-danger d-block" />
    <asp:Panel ID="pnlLesson" runat="server" Visible="false">
      <nav aria-label="breadcrumb" class="mb-3">
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a id="aCourse" runat="server">Course</a></li>
          <li class="breadcrumb-item"><asp:Literal ID="litModuleName" runat="server" /></li>
          <li class="breadcrumb-item active"><asp:Literal ID="litLessonTitle" runat="server" /></li>
        </ol>
      </nav>
      <h1 class="mb-4"><asp:Literal ID="litTitle" runat="server" /></h1>
      <asp:Panel ID="pnlVideo" runat="server" CssClass="mb-5">
        <h4>Video</h4>
        <video id="lessonVideo" controls class="w-100 rounded" style="max-height:480px">
          <source id="videoSrc" runat="server" />
        </video>
      </asp:Panel>
      <asp:Panel ID="pnlNotes" runat="server" CssClass="mb-5">
        <h4>Study Notes</h4>
        <div class="card p-4"><asp:Literal ID="litNoteContent" runat="server" /></div>
        <a id="aNoteFile" runat="server" class="btn btn-outline-secondary btn-sm mt-2">Download PDF</a>
      </asp:Panel>
      <asp:Panel ID="pnlQuiz" runat="server" CssClass="mb-4">
        <h4>Quiz</h4>
        <asp:Repeater ID="rptQuizzes" runat="server">
          <ItemTemplate>
            <a href='<%# "~/Courses/Quiz.aspx?quizId=" + Eval("QuizId") %>' class="btn btn-outline-primary me-2">
              <%# System.Web.HttpUtility.HtmlEncode((string)Eval("QuizTitle")) %>
            </a>
          </ItemTemplate>
        </asp:Repeater>
      </asp:Panel>
      <div class="d-flex gap-3 mt-4">
        <asp:Button ID="btnMarkComplete" runat="server" Text="Mark as Complete" CssClass="btn btn-aaram-primary" OnClick="MarkComplete_Click" />
        <a id="aNext" runat="server" class="btn btn-outline-secondary">Next Lesson</a>
      </div>
    </asp:Panel>
  </div>
</asp:Content>
