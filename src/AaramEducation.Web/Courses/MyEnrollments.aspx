<%@ Page Title="My Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyEnrollments.aspx.cs" Inherits="AaramEducation.Web.Courses.MyEnrollmentsPage" %>
<%@ Import Namespace="AaramEducation.Core.Entities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container py-4">
  <h1 class="mb-4">My Courses</h1>

  <% if (rptEnrollments.Items.Count == 0) { %>
    <div class="text-center py-5">
      <p class="text-muted fs-5">You haven't enrolled in any courses yet.</p>
      <a href='<%= ResolveUrl("~/Courses/Index.aspx") %>' class="btn btn-primary">Browse courses</a>
    </div>
  <% } %>

  <asp:Repeater ID="rptEnrollments" runat="server">
    <ItemTemplate>
      <div class="enrollment-card">
        <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
          <div>
            <h5 class="mb-1">
              <a href='<%# ResolveUrl("~/Courses/Details.aspx?id=" + Eval("CourseId")) %>'><%# Server.HtmlEncode((string)Eval("Course.CourseName")) %></a>
            </h5>
            <small class="text-muted"><%# Server.HtmlEncode((string)Eval("Course.Subject")) %> &middot; <%# Server.HtmlEncode((string)Eval("Course.DifficultyLevel")) %></small>
          </div>
          <span class='<%# Eval("EnrollmentStatus").ToString() == "Completed" ? "text-success" : Eval("EnrollmentStatus").ToString() == "Dropped" ? "text-danger" : "text-primary" %> fw-semibold small'><%# Eval("EnrollmentStatus") %></span>
        </div>

        <div class="mt-3 mb-1">
          <div class="progress-bar-aaram">
            <div class="fill" style='width:<%# (((Enrollment)Container.DataItem).CourseProgress?.PercentComplete ?? 0f).ToString("F0") %>%'></div>
          </div>
        </div>
        <div class="d-flex justify-content-between align-items-center">
          <small class="text-muted"><%# (((Enrollment)Container.DataItem).CourseProgress?.PercentComplete ?? 0f).ToString("F0") %>% complete</small>
          <a href='<%# ResolveUrl("~/Courses/Details.aspx?id=" + Eval("CourseId")) %>'
             class="btn btn-sm btn-outline-primary">Continue</a>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
</div>

</asp:Content>
