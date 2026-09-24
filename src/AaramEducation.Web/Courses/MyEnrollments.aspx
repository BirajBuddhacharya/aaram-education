<%@ Page Title="My Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyEnrollments.aspx.cs" Inherits="AaramEducation.Web.Courses.MyEnrollmentsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <h1 class="t-h2 mb-4">My Courses</h1>
    <div class="row g-4">
      <asp:Repeater ID="rptEnrollments" runat="server">
        <ItemTemplate>
          <div class="col-md-4">
            <div class="card h-100 p-3">
              <h3 class="t-h4"><%# System.Web.HttpUtility.HtmlEncode(Eval("Course.CourseName")) %></h3>
              <p class="t-small text-muted"><%# System.Web.HttpUtility.HtmlEncode(Eval("Course.Subject")) %></p>
              <div class="progress mb-2" style="height:6px">
                <div class="progress-bar" style='width:<%# Eval("CourseProgress.PercentComplete") ?? 0 %>%'></div>
              </div>
              <p class="t-small mb-3"><%# Eval("CourseProgress.PercentComplete") ?? 0 %>% complete</p>
              <a href='<%# "~/Courses/Details.aspx?id=" + Eval("CourseId") %>' class="btn btn-sm btn-aaram-primary">Continue</a>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>
