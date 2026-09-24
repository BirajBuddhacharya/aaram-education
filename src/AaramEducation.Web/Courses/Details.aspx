<%@ Page Title="Course Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="AaramEducation.Web.Courses.CourseDetailsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <asp:Label ID="lblNotFound" runat="server" Visible="false">
      <div class="alert alert-warning">Course not found.</div>
    </asp:Label>
    <div id="courseContent" runat="server">
      <h1 class="t-h2 mb-1"><asp:Literal ID="litCourseName" runat="server" /></h1>
      <p class="t-small text-muted mb-3">
        <asp:Literal ID="litSubject" runat="server" /> &bull; <asp:Literal ID="litDifficulty" runat="server" />
        &bull; by <asp:Literal ID="litTutor" runat="server" />
      </p>
      <p class="t-body mb-4"><asp:Literal ID="litDescription" runat="server" /></p>

      <asp:Panel ID="pnlEnrolled" runat="server" Visible="false">
        <div class="alert alert-success d-flex align-items-center gap-2 mb-4">
          <span>You are enrolled &mdash; <asp:Literal ID="litProgress" runat="server" />% complete</span>
          <asp:Button ID="btnDrop" runat="server" Text="Drop course" CssClass="btn btn-sm btn-outline-danger ms-auto" OnClick="Drop_Click" />
        </div>
      </asp:Panel>
      <asp:Panel ID="pnlNotEnrolled" runat="server" Visible="false">
        <asp:Button ID="btnEnroll" runat="server" Text="Enroll for free" CssClass="btn btn-aaram-primary mb-4" OnClick="Enroll_Click" />
      </asp:Panel>
      <asp:Panel ID="pnlLoginToEnroll" runat="server" Visible="false">
        <a href="~/Account/Login.aspx" runat="server" class="btn btn-aaram-primary mb-4">Sign in to enroll</a>
      </asp:Panel>

      <h2 class="t-h4 mb-3">Course content</h2>
      <asp:Repeater ID="rptModules" runat="server">
        <ItemTemplate>
          <div class="card p-3 mb-3">
            <h3 class="t-h4 mb-2"><%# System.Web.HttpUtility.HtmlEncode(Eval("ModuleName")) %></h3>
            <ul class="list-unstyled mb-0">
              <asp:Repeater ID="rptLessons" runat="server" DataSource='<%# Eval("Lessons") %>'>
                <ItemTemplate>
                  <li class="py-1 border-bottom">
                    <a href='<%# "~/Lessons/Show.aspx?id=" + Eval("LessonId") %>' class="link-aaram">
                      <%# System.Web.HttpUtility.HtmlEncode(Eval("LessonTitle")) %>
                    </a>
                    <%# (bool)Eval("IsFreeSample") ? "<span class='badge bg-success ms-2'>Free</span>" : "" %>
                  </li>
                </ItemTemplate>
              </asp:Repeater>
            </ul>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
  </div>
</asp:Content>
