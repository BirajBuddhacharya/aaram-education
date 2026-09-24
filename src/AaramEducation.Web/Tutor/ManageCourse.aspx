<%@ Page Title="Manage Course" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCourse.aspx.cs" Inherits="AaramEducation.Web.Tutor.ManageCourse" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h1><asp:Literal ID="litCourseName" runat="server" /></h1>
      <div class="d-flex gap-2">
        <a href='<%# "~/Tutor/CourseForm.aspx?id=" + CourseId %>' runat="server" class="btn btn-outline-secondary">Edit Course</a>
        <asp:HyperLink ID="lnkNewModule" runat="server" CssClass="btn btn-aaram-primary">+ Add Module</asp:HyperLink>
      </div>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:Repeater ID="rptModules" runat="server" OnItemCommand="rptModules_ItemCommand">
      <ItemTemplate>
        <div class="card mb-4 p-4">
          <div class="d-flex justify-content-between align-items-start mb-3">
            <div>
              <h5 class="mb-0"><%# Eval("ModuleName") %></h5>
              <p class="t-small text-muted mb-0"><%# Eval("ModuleDescription") %></p>
            </div>
            <div class="d-flex gap-2">
              <asp:Button runat="server" Text="Edit Module" CssClass="btn btn-sm btn-outline-secondary"
                CommandName="EditModule" CommandArgument='<%# Eval("ModuleId") %>' />
              <asp:Button runat="server" Text="Delete Module" CssClass="btn btn-sm btn-danger"
                CommandName="DeleteModule" CommandArgument='<%# Eval("ModuleId") %>'
                OnClientClick="return confirm('Delete this module and all lessons?');" />
            </div>
          </div>
          <asp:Repeater ID="rptLessons" runat="server" DataSource='<%# Eval("Lessons") %>' OnItemCommand="rptLessons_ItemCommand">
            <ItemTemplate>
              <div class="d-flex justify-content-between align-items-center py-2 border-top">
                <span><%# Eval("SequenceOrder") %>. <%# Eval("LessonTitle") %></span>
                <div class="d-flex gap-2">
                  <asp:Button runat="server" Text="Edit Lesson" CssClass="btn btn-sm btn-outline-primary"
                    CommandName="EditLesson" CommandArgument='<%# Eval("LessonId") %>' />
                  <asp:Button runat="server" Text="Delete" CssClass="btn btn-sm btn-danger"
                    CommandName="DeleteLesson" CommandArgument='<%# Eval("LessonId") %>'
                    OnClientClick="return confirm('Delete this lesson?');" />
                </div>
              </div>
            </ItemTemplate>
          </asp:Repeater>
          <div class="mt-3">
            <asp:Button runat="server" Text="+ Add Lesson" CssClass="btn btn-sm btn-outline-primary"
              CommandName="AddLesson" CommandArgument='<%# Eval("ModuleId") %>' />
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</asp:Content>
