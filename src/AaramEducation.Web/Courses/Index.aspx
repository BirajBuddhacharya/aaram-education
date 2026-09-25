<%@ Page Title="Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Courses.CoursesIndexPage" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="AaramEducation.Core.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container" style="padding-top:40px;padding-bottom:80px">

  <nav class="crumbs mb-4" aria-label="Breadcrumb">
    <a href='<%= ResolveUrl("~/Default.aspx") %>'>Home</a>
    <span class="sep">/</span>
    <span>Courses</span>
  </nav>

  <h1 class="t-h1 mb-3">Courses</h1>
  <p class="t-body mb-4" style="max-width:34em">
    Every course is split into lessons of 5 to 10 minutes. Stop whenever you like
    and pick up where you left off.
  </p>

  <div class="d-flex justify-content-between align-items-center flex-wrap gap-3 mb-4">
    <div class="d-flex flex-wrap gap-2">
      <a href='<%= ResolveUrl("~/Courses/Index.aspx" + (string.IsNullOrEmpty(Request.QueryString["difficulty"]) ? "" : "?difficulty=" + Server.UrlEncode(Request.QueryString["difficulty"]))) %>'
         class="chip <%= string.IsNullOrEmpty(Request.QueryString["subject"]) ? "active" : "" %>">All</a>
      <asp:Repeater ID="rptSubjects" runat="server">
        <ItemTemplate>
          <a href='<%# ResolveUrl("~/Courses/Index.aspx?subject=" + Server.UrlEncode((string)Container.DataItem) + (string.IsNullOrEmpty(Request.QueryString["difficulty"]) ? "" : "&difficulty=" + Server.UrlEncode(Request.QueryString["difficulty"]))) %>'
             class='chip <%# Request.QueryString["subject"] == (string)Container.DataItem ? "active" : "" %>'><%# Server.HtmlEncode((string)Container.DataItem) %></a>
        </ItemTemplate>
      </asp:Repeater>
    </div>

    <div class="d-flex flex-wrap gap-2 align-items-center">
      <span class="t-xs me-1">Level</span>
      <a href='<%= ResolveUrl("~/Courses/Index.aspx?" + (string.IsNullOrEmpty(Request.QueryString["subject"]) ? "" : "subject=" + Server.UrlEncode(Request.QueryString["subject"]) + "&") + (Request.QueryString["difficulty"] == "Beginner" ? "" : "difficulty=Beginner")) %>'
         class="chip <%= Request.QueryString["difficulty"] == "Beginner" ? "active" : "" %>">Beginner</a>
      <a href='<%= ResolveUrl("~/Courses/Index.aspx?" + (string.IsNullOrEmpty(Request.QueryString["subject"]) ? "" : "subject=" + Server.UrlEncode(Request.QueryString["subject"]) + "&") + (Request.QueryString["difficulty"] == "Intermediate" ? "" : "difficulty=Intermediate")) %>'
         class="chip <%= Request.QueryString["difficulty"] == "Intermediate" ? "active" : "" %>">Intermediate</a>
      <a href='<%= ResolveUrl("~/Courses/Index.aspx?" + (string.IsNullOrEmpty(Request.QueryString["subject"]) ? "" : "subject=" + Server.UrlEncode(Request.QueryString["subject"]) + "&") + (Request.QueryString["difficulty"] == "Advanced" ? "" : "difficulty=Advanced")) %>'
         class="chip <%= Request.QueryString["difficulty"] == "Advanced" ? "active" : "" %>">Advanced</a>
    </div>
  </div>

  <% if (rptCourses.Items.Count == 0) { %>
    <div class="card-aaram text-center" style="padding:56px 24px">
      <p class="t-h4 mb-2">No courses match those filters.</p>
      <p class="t-small mb-4">Try a different subject or level.</p>
      <div><a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="btn-aaram btn-aaram-outline btn-aaram-sm">Clear filters</a></div>
    </div>
  <% } %>

  <div class="row row-cols-1 row-cols-md-2 row-cols-xl-4 g-4">
    <asp:Repeater ID="rptCourses" runat="server">
      <ItemTemplate>
        <div class="col">
          <a class="course-card" href='<%# ResolveUrl("~/Courses/Details.aspx?id=" + Eval("CourseId")) %>'>
            <div class="d-flex justify-content-between align-items-center gap-2">
              <span class="course-card-subject">
                <span class="subject-dot <%# ((string)Eval("Subject")).ToLower() == "science" ? "subject-science" : ((string)Eval("Subject")).ToLower() == "english" ? "subject-english" : (((string)Eval("Subject")).ToLower() == "computing" || ((string)Eval("Subject")).ToLower() == "computer science") ? "subject-computing" : "subject-maths" %>"></span><%# Server.HtmlEncode((string)Eval("Subject")) %>
              </span>
              <%# ((Course)Container.DataItem).Modules.Any(m => m.Lessons.Any(l => l.IsFreeSample)) ? "<span class=\"pill-free\">Free sample</span>" : "" %>
            </div>

            <div class="t-h4"><%# Server.HtmlEncode((string)Eval("CourseName")) %></div>

            <p class="t-small mb-0 flex-grow-1">
              <%# Server.HtmlEncode((Eval("CourseDescription") as string ?? "").Length > 110 ? (Eval("CourseDescription") as string ?? "").Substring(0, 110) + "…" : (Eval("CourseDescription") as string ?? "")) %>
            </p>

            <div class="t-xs">
              <%# ((Course)Container.DataItem).Modules.Sum(m => m.Lessons.Count) %> lesson<%# ((Course)Container.DataItem).Modules.Sum(m => m.Lessons.Count) == 1 ? "" : "s" %>,
              <%# Server.HtmlEncode(((string)Eval("DifficultyLevel")).ToLower()) %>,
              taught by <%# Server.HtmlEncode(((Course)Container.DataItem).CreatedBy.FirstName) %> <%# ((Course)Container.DataItem).CreatedBy.LastName.Length > 0 ? Server.HtmlEncode(((Course)Container.DataItem).CreatedBy.LastName.Substring(0, 1)) + "." : "" %>
            </div>
          </a>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</div>

</asp:Content>
