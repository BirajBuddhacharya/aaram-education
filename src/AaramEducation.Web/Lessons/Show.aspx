<%@ Page Title="Lesson" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="AaramEducation.Web.Lessons.Show" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="container" style="padding-top:32px;padding-bottom:72px">

  <nav class="crumbs mb-4" aria-label="Breadcrumb">
    <asp:Literal ID="litBreadcrumb" runat="server" />
  </nav>

  <asp:Literal ID="litMessage" runat="server" />

  <div class="row g-4 g-lg-5">

    <!-- ══ Main ══════════════════════════════════════════════════════════ -->
    <div class="col-lg-8">

      <asp:Panel ID="pnlVideo" runat="server" CssClass="video-wrapper mb-3">
        <div class="ratio ratio-16x9">
          <asp:Literal ID="litVideo" runat="server" />
        </div>
      </asp:Panel>

      <p class="t-xs mb-2"><asp:Literal ID="litVideoTitle" runat="server" /></p>

      <div class="d-flex justify-content-between align-items-start gap-3 flex-wrap mb-4">
        <h1 class="lesson-title mb-0"><asp:Literal ID="litTitle" runat="server" /></h1>
        <span class="d-flex gap-2">
          <asp:HyperLink ID="lnkPrev" runat="server" CssClass="btn-aaram btn-aaram-outline btn-aaram-sm" Visible="false">Previous</asp:HyperLink>
          <asp:HyperLink ID="lnkNext" runat="server" CssClass="btn-aaram btn-aaram-outline btn-aaram-sm" Visible="false">Next lesson</asp:HyperLink>
        </span>
      </div>

      <div class="segmented mb-4" role="tablist">
        <button type="button" class="active" data-tab="notes" role="tab" aria-selected="true">Notes</button>
        <button type="button" data-tab="quiz" role="tab" aria-selected="false">Practice quiz</button>
      </div>

      <div id="tab-notes" class="prose">
        <asp:Panel ID="pnlNote" runat="server" CssClass="mb-4">
          <div class="t-h4 mb-2"><asp:Literal ID="litNoteTitle" runat="server" /></div>
          <p><asp:Literal ID="litNoteContent" runat="server" /></p>
        </asp:Panel>

        <asp:Panel ID="pnlProgress" runat="server" CssClass="mt-4 d-flex align-items-center gap-3 flex-wrap">
          <asp:Button ID="btnComplete" runat="server" Text="&#10003; Mark as complete" CssClass="btn-aaram btn-aaram-primary" OnClick="btnComplete_Click" />
          <asp:Literal ID="litProgressStatus" runat="server" />
        </asp:Panel>
      </div>

      <div id="tab-quiz" class="d-none">
        <asp:Panel ID="pnlQuizzes" runat="server" CssClass="d-flex flex-column gap-3">
          <asp:Repeater ID="rptQuizzes" runat="server">
            <ItemTemplate>
              <a href='<%# ResolveUrl("~/Quiz/Start.aspx?id=" + Eval("QuizId")) %>'
                 class="course-card flex-row align-items-center justify-content-between">
                <span>
                  <span class="t-h4 d-block"><%# Server.HtmlEncode((string)Eval("QuizTitle")) %></span>
                  <span class="t-xs">Up to <%# Eval("MaxAttempts") %> attempts</span>
                </span>
                <span class="btn-aaram btn-aaram-primary btn-aaram-sm">Start</span>
              </a>
            </ItemTemplate>
          </asp:Repeater>
        </asp:Panel>
      </div>
    </div>

    <!-- ══ Sidebar ═══════════════════════════════════════════════════════ -->
    <div class="col-lg-4">
      <div class="sticky-top" style="top:96px">
        <div class="card-tint">
          <div class="t-h4 mb-2">Still stuck?</div>
          <p class="t-small mb-3">
            Ask the tutor who made this course to walk you through it.
          </p>
          <asp:HyperLink ID="lnkBack" runat="server" CssClass="btn-aaram btn-aaram-dark btn-aaram-sm">Back to course</asp:HyperLink>
        </div>
      </div>
    </div>

  </div>
</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptsContent" runat="server">
  <script src='<%= ResolveUrl("~/wwwroot/js/lesson-player.js") %>'></script>
  <script>
    document.querySelectorAll('.segmented [data-tab]').forEach(btn => {
      btn.addEventListener('click', function () {
        document.querySelectorAll('.segmented [data-tab]').forEach(b => {
          b.classList.remove('active');
          b.setAttribute('aria-selected', 'false');
        });
        this.classList.add('active');
        this.setAttribute('aria-selected', 'true');
        document.getElementById('tab-notes').classList.toggle('d-none', this.dataset.tab !== 'notes');
        document.getElementById('tab-quiz').classList.toggle('d-none', this.dataset.tab !== 'quiz');
      });
    });
  </script>
</asp:Content>
