<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AaramEducation.Web.DefaultPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="hero-section py-5">
    <div class="container-xl text-center py-5">
      <h1 class="t-h1 mb-3">Learn at your own pace</h1>
      <p class="t-lead mb-4">aaram brings you stress-free online education with gamification, quizzes, and progress tracking.</p>
      <a href="Courses/Index.aspx" class="btn btn-aaram-primary btn-lg me-2">Browse Courses</a>
      <a href="Account/Register.aspx" class="btn btn-outline-primary btn-lg">Get started free</a>
    </div>
  </div>
  <div class="container-xl py-5">
    <div class="row g-4 text-center">
      <div class="col-md-4">
        <div class="card h-100 p-4">
          <h3 class="t-h4">Learn</h3>
          <p class="t-body">Access video lessons and study notes curated by expert tutors.</p>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100 p-4">
          <h3 class="t-h4">Practice</h3>
          <p class="t-body">Test your knowledge with quizzes and track your progress.</p>
        </div>
      </div>
      <div class="col-md-4">
        <div class="card h-100 p-4">
          <h3 class="t-h4">Earn</h3>
          <p class="t-body">Collect XP, unlock badges, and climb the leaderboard.</p>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
