<%@ Page Title="Manage Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="AaramEducation.Web.Admin.CoursesAdminPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1>Courses</h1>
      <a href="~/Admin/Dashboard.aspx" runat="server" class="btn btn-outline-secondary">Back</a>
    </div>
    <div class="table-responsive">
      <table class="table">
        <thead><tr><th>Course</th><th>Subject</th><th>Tutor</th><th>Published</th><th>Enrollments</th><th></th></tr></thead>
        <tbody>
          <asp:Repeater ID="rptCourses" runat="server" OnItemCommand="Courses_Command">
            <ItemTemplate>
              <tr>
                <td><%# System.Web.HttpUtility.HtmlEncode((string)Eval("CourseName")) %></td>
                <td><%# System.Web.HttpUtility.HtmlEncode((string)Eval("Subject")) %></td>
                <td><%# System.Web.HttpUtility.HtmlEncode(Eval("CreatedBy.FirstName") + " " + Eval("CreatedBy.LastName")) %></td>
                <td><%# (bool)Eval("IsPublished") ? "Yes" : "No" %></td>
                <td><%# ((System.Collections.Generic.ICollection<AaramEducation.Core.Entities.Enrollment>)Eval("Enrollments")).Count %></td>
                <td>
                  <asp:Button runat="server" CommandName="Toggle" CommandArgument='<%# Eval("CourseId") %>'
                    Text='<%# (bool)Eval("IsPublished") ? "Unpublish" : "Publish" %>'
                    CssClass="btn btn-sm btn-outline-secondary" />
                </td>
              </tr>
            </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </div>
  </div>
</asp:Content>
