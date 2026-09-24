<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Admin.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <h1 class="mb-4">Admin Dashboard</h1>
    <div class="row g-4">
      <div class="col-sm-6 col-xl-3">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-primary"><asp:Literal ID="litUsers" runat="server" /></div>
          <div class="t-small mt-1">Total Users</div>
        </div>
      </div>
      <div class="col-sm-6 col-xl-3">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-primary"><asp:Literal ID="litCourses" runat="server" /></div>
          <div class="t-small mt-1">Published Courses</div>
        </div>
      </div>
      <div class="col-sm-6 col-xl-3">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-primary"><asp:Literal ID="litEnrollments" runat="server" /></div>
          <div class="t-small mt-1">Enrollments</div>
        </div>
      </div>
      <div class="col-sm-6 col-xl-3">
        <div class="card text-center p-4">
          <div class="display-4 fw-bold text-warning"><asp:Literal ID="litPending" runat="server" /></div>
          <div class="t-small mt-1">Pending Guestbook</div>
        </div>
      </div>
    </div>
    <div class="row g-3 mt-4">
      <div class="col-auto"><a href="~/Admin/Users.aspx" runat="server" class="btn btn-outline-primary">Manage Users</a></div>
      <div class="col-auto"><a href="~/Admin/Courses.aspx" runat="server" class="btn btn-outline-primary">Manage Courses</a></div>
      <div class="col-auto"><a href="~/Admin/Guestbook.aspx" runat="server" class="btn btn-outline-warning">Moderate Guestbook</a></div>
    </div>
  </div>
</asp:Content>
