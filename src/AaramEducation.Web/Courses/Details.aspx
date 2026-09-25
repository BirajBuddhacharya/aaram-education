<%@ Page Title="Course Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="AaramEducation.Web.Courses.CourseDetailsPage" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<asp:Label ID="lblNotFound" runat="server" Visible="false">
  <div class="container py-5"><div class="alert alert-warning">Course not found.</div></div>
</asp:Label>

<div id="courseContent" runat="server">

  <div class="course-banner">
    <div class="container">
      <span class="subject-badge mb-2"><asp:Literal ID="litSubject" runat="server" /></span>
      <h1 class="mb-1"><asp:Literal ID="litCourseName" runat="server" /></h1>
      <p class="mb-2 opacity-75">by <asp:Literal ID="litTutor" runat="server" /></p>
      <span class="difficulty-badge <%= litDifficulty.Text.ToLower() == "beginner" ? "difficulty-beginner" : litDifficulty.Text.ToLower() == "intermediate" ? "difficulty-intermediate" : "difficulty-advanced" %>">
        <asp:Literal ID="litDifficulty" runat="server" />
      </span>
    </div>
  </div>

  <div class="container pb-5">
    <div class="row g-4">
      <div class="col-lg-8">
        <h4 class="mb-3">Curriculum</h4>
        <div class="accordion" id="curriculumAccordion">
          <asp:Repeater ID="rptModules" runat="server">
            <ItemTemplate>
              <div class="accordion-item">
                <h2 class="accordion-header">
                  <button class="accordion-button collapsed" type="button"
                          data-bs-toggle="collapse" data-bs-target='<%# "#module-" + Eval("ModuleId") %>' aria-expanded="false">
                    <%# Server.HtmlEncode((string)Eval("ModuleName")) %>
                    <span class="ms-2 text-muted small">(<%# Eval("Lessons.Count") %> lesson(s))</span>
                  </button>
                </h2>
                <div id='<%# "module-" + Eval("ModuleId") %>' class="accordion-collapse collapse" data-bs-parent="#curriculumAccordion">
                  <div class="accordion-body p-0">
                    <ul class="list-group list-group-flush">
                      <asp:Repeater ID="rptLessons" runat="server" DataSource='<%# Eval("Lessons") %>'>
                        <ItemTemplate>
                          <li class="list-group-item d-flex align-items-center gap-2 py-2">
                            <%# (!pnlEnrolled.Visible && !(bool)Eval("IsFreeSample"))
                                  ? "<span title=\"Enroll to unlock\">&#128274;</span>"
                                  : "<span>&#127891;</span>" %>
                            <%# (!pnlEnrolled.Visible && !(bool)Eval("IsFreeSample"))
                                  ? "<span class=\"text-muted\">" + Server.HtmlEncode((string)Eval("LessonTitle")) + "</span>"
                                  : "<a href=\"" + ResolveUrl("~/Lessons/Show.aspx?id=" + Eval("LessonId")) + "\">" + Server.HtmlEncode((string)Eval("LessonTitle")) + "</a>" %>
                            <%# (bool)Eval("IsFreeSample")
                                  ? "<span class=\"badge bg-success ms-auto\" style=\"font-size:.65rem\">Free preview</span>"
                                  : "" %>
                          </li>
                        </ItemTemplate>
                      </asp:Repeater>
                    </ul>
                  </div>
                </div>
              </div>
            </ItemTemplate>
          </asp:Repeater>
        </div>

        <div class="mt-4">
          <h4>About this course</h4>
          <p><asp:Literal ID="litDescription" runat="server" /></p>
        </div>
      </div>

      <div class="col-lg-4">
        <div class="sticky-top" style="top:1rem">
          <div class="course-card">
            <div class="course-card-body">

              <asp:Panel ID="pnlEnrolled" runat="server" Visible="false">
                <p class="mb-2 fw-semibold text-success">You are enrolled</p>
                <div class="progress-bar-aaram mb-2">
                  <div class="fill" style='width:<%= litProgress.Text %>%'></div>
                </div>
                <small class="text-muted"><asp:Literal ID="litProgress" runat="server" />% complete</small>
                <hr />
                <a href='<%= ResolveUrl("~/Courses/MyEnrollments.aspx") %>' class="btn btn-outline-primary w-100 mb-2">My courses</a>
                <asp:Button ID="btnDrop" runat="server" Text="Drop course"
                            CssClass="btn btn-outline-danger w-100 btn-sm"
                            OnClick="Drop_Click" OnClientClick="return confirm('Drop this course?');" />
              </asp:Panel>

              <asp:Panel ID="pnlNotEnrolled" runat="server" Visible="false">
                <p class="text-muted small mb-3">Enroll to access all lessons, quizzes, and track your progress.</p>
                <asp:Button ID="btnEnroll" runat="server" Text="Enroll for free"
                            CssClass="btn btn-primary w-100" OnClick="Enroll_Click" />
              </asp:Panel>

              <asp:Panel ID="pnlLoginToEnroll" runat="server" Visible="false">
                <p class="text-muted small mb-3">Enroll to access all lessons, quizzes, and track your progress.</p>
                <a href='<%= ResolveUrl("~/Account/Login.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl)) %>' class="btn btn-primary w-100">Sign in to enroll</a>
              </asp:Panel>

            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

</div>

</asp:Content>
