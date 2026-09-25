<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AaramEducation.Web.Admin.Dashboard" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <h1 class="mb-4">Admin Dashboard</h1>

    <div class="row g-3 mb-4">
      <div class="col-6 col-md-4 col-lg-2">
        <div class="card text-center h-100">
          <div class="card-body">
            <div class="fs-2 fw-bold text-primary"><asp:Literal ID="litUsers" runat="server" /></div>
            <div class="text-muted small">Total Users</div>
          </div>
        </div>
      </div>
      <div class="col-6 col-md-4 col-lg-2">
        <div class="card text-center h-100">
          <div class="card-body">
            <div class="fs-2 fw-bold text-success"><asp:Literal ID="litCourses" runat="server" /></div>
            <div class="text-muted small">Published Courses</div>
          </div>
        </div>
      </div>
      <div class="col-6 col-md-4 col-lg-2">
        <div class="card text-center h-100">
          <div class="card-body">
            <div class="fs-2 fw-bold text-info"><asp:Literal ID="litEnrollments" runat="server" /></div>
            <div class="text-muted small">Enrollments</div>
          </div>
        </div>
      </div>
      <div class="col-6 col-md-4 col-lg-2">
        <div class="card text-center h-100">
          <div class="card-body">
            <div class="fs-2 fw-bold text-warning"><asp:Literal ID="litPending" runat="server" /></div>
            <div class="text-muted small">Pending Guestbook</div>
          </div>
        </div>
      </div>
    </div>

    <div class="d-flex gap-2 flex-wrap">
      <a href='<%= ResolveUrl("~/Admin/Users.aspx") %>' class="btn btn-outline-primary">Manage Users</a>
      <a href='<%= ResolveUrl("~/Admin/Courses.aspx") %>' class="btn btn-outline-primary">Manage Courses</a>
      <a href='<%= ResolveUrl("~/Admin/Guestbook.aspx") %>' class="btn btn-outline-warning">Guestbook Moderation</a>
    </div>
  </div>
</asp:Content>
