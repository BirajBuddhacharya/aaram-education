<%@ Page Title="Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AaramEducation.Web.Courses.CoursesIndexPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex align-items-center justify-content-between mb-4">
      <h1 class="t-h2 mb-0">Courses</h1>
    </div>
    <%-- Filters --%>
    <form method="get" class="row g-2 mb-4">
      <div class="col-auto">
        <select name="subject" class="form-select form-select-sm">
          <option value="">All subjects</option>
          <asp:Repeater ID="rptSubjects" runat="server">
            <ItemTemplate><%# GetSubjectOption(Container.DataItem.ToString()) %></ItemTemplate>
          </asp:Repeater>
        </select>
      </div>
      <div class="col-auto">
        <select name="difficulty" class="form-select form-select-sm">
          <option value="">All levels</option>
          <option value="Beginner" <%# Request.QueryString["difficulty"] == "Beginner" ? "selected" : "" %>>Beginner</option>
          <option value="Intermediate" <%# Request.QueryString["difficulty"] == "Intermediate" ? "selected" : "" %>>Intermediate</option>
          <option value="Advanced" <%# Request.QueryString["difficulty"] == "Advanced" ? "selected" : "" %>>Advanced</option>
        </select>
      </div>
      <div class="col-auto">
        <button type="submit" class="btn btn-sm btn-outline-secondary">Filter</button>
      </div>
    </form>
    <div class="row g-4">
      <asp:Repeater ID="rptCourses" runat="server">
        <ItemTemplate>
          <div class="col-md-4">
            <div class="card h-100 p-3">
              <h3 class="t-h4"><%# System.Web.HttpUtility.HtmlEncode(Eval("CourseName")) %></h3>
              <p class="t-small text-muted mb-2"><%# System.Web.HttpUtility.HtmlEncode(Eval("Subject")) %> &bull; <%# System.Web.HttpUtility.HtmlEncode(Eval("DifficultyLevel")) %></p>
              <p class="t-small flex-grow-1"><%# System.Web.HttpUtility.HtmlEncode(Eval("CourseDescription")) %></p>
              <a href='<%# "~/Courses/Details.aspx?id=" + Eval("CourseId") %>' class="btn btn-sm btn-aaram-primary mt-auto">View course</a>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>
