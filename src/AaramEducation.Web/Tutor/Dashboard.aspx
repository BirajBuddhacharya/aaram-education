<%@ Page Title="My Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Tutor.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1>My Courses</h1>
      <a href="~/Tutor/CourseForm.aspx?id=0" runat="server" class="btn btn-aaram-primary">+ New Course</a>
    </div>
    <asp:Literal ID="litEmpty" runat="server" />
    <div class="row g-4">
      <asp:Repeater ID="rptCourses" runat="server">
        <ItemTemplate>
          <div class="col-md-6 col-xl-4">
            <div class="card h-100 p-4">
              <div class="mb-1">
                <span class="badge <%# (bool)Eval("IsPublished") ? "bg-success" : "bg-secondary" %>">
                  <%# (bool)Eval("IsPublished") ? "Published" : "Draft" %>
                </span>
              </div>
              <h5 class="mb-1"><%# Eval("CourseName") %></h5>
              <p class="t-small text-muted mb-3"><%# Eval("Subject") %> · <%# Eval("DifficultyLevel") %></p>
              <div class="d-flex gap-2 mt-auto">
                <a href='<%# "~/Tutor/ManageCourse.aspx?id=" + Eval("CourseId") %>' runat="server" class="btn btn-sm btn-outline-primary">Manage</a>
                <a href='<%# "~/Tutor/CourseForm.aspx?id=" + Eval("CourseId") %>' runat="server" class="btn btn-sm btn-outline-secondary">Edit</a>
              </div>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>
