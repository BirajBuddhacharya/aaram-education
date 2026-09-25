<%@ Page Title="Manage Course" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCourse.aspx.cs" Inherits="AaramEducation.Web.Tutor.ManageCourse" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container py-4">
    <div class="d-flex align-items-center justify-content-between mb-3">
      <div>
        <a href='<%= ResolveUrl("~/Tutor/Dashboard.aspx") %>' class="text-muted small">← My Courses</a>
        <h1 class="mb-0"><asp:Literal ID="litCourseName" runat="server" /></h1>
      </div>
      <div class="d-flex gap-2">
        <a href='<%= ResolveUrl("~/Tutor/CourseForm.aspx?id=" + CourseId) %>' class="btn btn-outline-secondary btn-sm">Edit Details</a>
      </div>
    </div>

    <div class="mb-3">
      <asp:HyperLink ID="lnkNewModule" runat="server" CssClass="btn btn-primary btn-sm">+ Add Module</asp:HyperLink>
    </div>

    <asp:Literal ID="litMessage" runat="server" />

    <asp:Repeater ID="rptModules" runat="server" OnItemCommand="rptModules_ItemCommand">
      <ItemTemplate>
        <div class="card mb-3">
          <div class="card-header d-flex align-items-center justify-content-between">
            <span class="fw-semibold">#<%# Eval("SequenceOrder") %> — <%# Eval("ModuleName") %></span>
            <div class="d-flex gap-2">
              <asp:LinkButton runat="server" Text="+ Lesson" CssClass="btn btn-sm btn-outline-primary"
                CommandName="AddLesson" CommandArgument='<%# Eval("ModuleId") %>' />
              <asp:LinkButton runat="server" Text="Edit" CssClass="btn btn-sm btn-outline-secondary"
                CommandName="EditModule" CommandArgument='<%# Eval("ModuleId") %>' />
              <asp:LinkButton runat="server" Text="Delete" CssClass="btn btn-sm btn-outline-danger"
                CommandName="DeleteModule" CommandArgument='<%# Eval("ModuleId") %>'
                OnClientClick="return confirm('Delete this module and all lessons?');" />
            </div>
          </div>
          <ul class="list-group list-group-flush">
            <asp:Repeater ID="rptLessons" runat="server" DataSource='<%# Eval("Lessons") %>' OnItemCommand="rptLessons_ItemCommand">
              <ItemTemplate>
                <li class="list-group-item d-flex align-items-center justify-content-between">
                  <span>
                    #<%# Eval("SequenceOrder") %> — <%# Eval("LessonTitle") %>
                    <%# (bool)Eval("IsFreeSample") ? "<span class=\"badge bg-info ms-1\">Free</span>" : "" %>
                  </span>
                  <div class="d-flex gap-2">
                    <asp:LinkButton runat="server" Text="Edit" CssClass="btn btn-sm btn-outline-secondary"
                      CommandName="EditLesson" CommandArgument='<%# Eval("LessonId") %>' />
                    <asp:LinkButton runat="server" Text="Delete" CssClass="btn btn-sm btn-outline-danger"
                      CommandName="DeleteLesson" CommandArgument='<%# Eval("LessonId") %>'
                      OnClientClick="return confirm('Delete this lesson?');" />
                  </div>
                </li>
              </ItemTemplate>
            </asp:Repeater>
          </ul>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</asp:Content>
