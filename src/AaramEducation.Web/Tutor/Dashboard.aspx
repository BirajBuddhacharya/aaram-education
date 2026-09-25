<%@ Page Title="My Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Tutor.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1>My Courses</h1>
      <a href='<%= ResolveUrl("~/Tutor/CourseForm.aspx?id=0") %>' class="btn btn-primary">+ Create Course</a>
    </div>

    <asp:Literal ID="litEmpty" runat="server" />

    <asp:Repeater ID="rptCourses" runat="server">
      <HeaderTemplate>
        <div class="table-responsive">
          <table class="table align-middle">
            <thead>
              <tr>
                <th>Course</th>
                <th>Subject</th>
                <th>Level</th>
                <th>Status</th>
                <th>Enrolled</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
      </HeaderTemplate>
      <ItemTemplate>
              <tr>
                <td>
                  <a href='<%# ResolveUrl("~/Tutor/ManageCourse.aspx?id=" + Eval("CourseId")) %>' class="fw-semibold text-decoration-none">
                    <%# Eval("CourseName") %>
                  </a>
                </td>
                <td><%# Eval("Subject") %></td>
                <td><%# Eval("DifficultyLevel") %></td>
                <td>
                  <span class='badge <%# (bool)Eval("IsPublished") ? "bg-success" : "bg-secondary" %>'><%# (bool)Eval("IsPublished") ? "Published" : "Draft" %></span>
                </td>
                <td><%# Eval("Enrollments.Count") %></td>
                <td class="text-end">
                  <a href='<%# ResolveUrl("~/Tutor/ManageCourse.aspx?id=" + Eval("CourseId")) %>' class="btn btn-sm btn-outline-primary">Manage</a>
                  <a href='<%# ResolveUrl("~/Tutor/CourseForm.aspx?id=" + Eval("CourseId")) %>' class="btn btn-sm btn-outline-secondary">Edit</a>
                </td>
              </tr>
      </ItemTemplate>
      <FooterTemplate>
            </tbody>
          </table>
        </div>
      </FooterTemplate>
    </asp:Repeater>
  </div>
</asp:Content>
