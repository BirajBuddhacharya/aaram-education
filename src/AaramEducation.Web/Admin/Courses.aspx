<%@ Page Title="Manage Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="AaramEducation.Web.Admin.CoursesAdminPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1>Courses</h1>
      <a href='<%= ResolveUrl("~/Admin/Dashboard.aspx") %>' class="btn btn-outline-secondary btn-sm">← Dashboard</a>
    </div>

    <div class="table-responsive">
      <table class="table table-hover align-middle">
        <thead class="table-light">
          <tr>
            <th>Course</th>
            <th>Tutor</th>
            <th>Subject</th>
            <th>Difficulty</th>
            <th>Published</th>
            <th>Enrollments</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptCourses" runat="server" OnItemCommand="Courses_Command">
            <ItemTemplate>
              <tr>
                <td class="fw-semibold"><%# System.Web.HttpUtility.HtmlEncode((string)Eval("CourseName")) %></td>
                <td class="text-muted small"><%# System.Web.HttpUtility.HtmlEncode(Eval("CreatedBy.FirstName") + " " + Eval("CreatedBy.LastName")) %></td>
                <td><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Subject")) %></td>
                <td><%# System.Web.HttpUtility.HtmlEncode((string)Eval("DifficultyLevel")) %></td>
                <td>
                  <span class='<%# (bool)Eval("IsPublished") ? "badge bg-success" : "badge bg-secondary" %>'><%# (bool)Eval("IsPublished") ? "Published" : "Draft" %></span>
                </td>
                <td><%# ((System.Collections.Generic.ICollection<AaramEducation.Core.Entities.Enrollment>)Eval("Enrollments")).Count %></td>
                <td class="d-flex gap-1 flex-wrap">
                  <asp:Button runat="server" CommandName="Toggle" CommandArgument='<%# Eval("CourseId") %>'
                    Text='<%# (bool)Eval("IsPublished") ? "Unpublish" : "Publish" %>'
                    CssClass='<%# (bool)Eval("IsPublished") ? "btn btn-sm btn-outline-secondary" : "btn btn-sm btn-outline-success" %>' />
                </td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
