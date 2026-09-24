<%@ Page Title="Quiz" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="AaramEducation.Web.Courses.QuizPage" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5" style="max-width:720px">
    <asp:Label ID="lblError" runat="server" Visible="false" CssClass="alert alert-danger d-block" />
    <asp:Panel ID="pnlQuiz" runat="server" Visible="false">
      <h1 class="mb-2"><asp:Literal ID="litTitle" runat="server" /></h1>
      <p class="t-small text-muted mb-4">Passing score: <asp:Literal ID="litPassing" runat="server" />%</p>
      <asp:HiddenField ID="hfQuizId" runat="server" />
      <asp:HiddenField ID="hfAttemptId" runat="server" />
      <asp:Repeater ID="rptQuestions" runat="server">
        <ItemTemplate>
          <div class="card mb-4 p-4">
            <p class="fw-semibold mb-3"><%# (int)Container.ItemIndex + 1 %>. <%# System.Web.HttpUtility.HtmlEncode((string)Eval("QuestionText")) %></p>
            <asp:Repeater ID="rptOptions" runat="server" DataSource='<%# Eval("Options") %>'>
              <ItemTemplate>
                <div class="form-check mb-2">
                  <input class="form-check-input" type="radio"
                    name='<%# "q_" + Eval("QuestionId") %>'
                    value='<%# Eval("OptionId") %>'
                    id='<%# "opt_" + Eval("OptionId") %>' />
                  <label class="form-check-label" for='<%# "opt_" + Eval("OptionId") %>'>
                    <%# System.Web.HttpUtility.HtmlEncode((string)Eval("OptionText")) %>
                  </label>
                </div>
              </ItemTemplate>
            </asp:Repeater>
          </div>
        </ItemTemplate>
      </asp:Repeater>
      <asp:Button ID="btnSubmit" runat="server" Text="Submit Quiz" CssClass="btn btn-aaram-primary" OnClick="Submit_Click" />
    </asp:Panel>
    <asp:Panel ID="pnlResult" runat="server" Visible="false">
      <div class="card p-5 text-center">
        <h2 class="mb-2">Quiz Complete!</h2>
        <div class="display-3 fw-bold mb-2"><asp:Literal ID="litScore" runat="server" />%</div>
        <asp:Literal ID="litResultMsg" runat="server" />
        <div class="mt-4">
          <a id="aBack" runat="server" class="btn btn-outline-secondary">Back to Lesson</a>
        </div>
      </div>
    </asp:Panel>
  </div>
</asp:Content>
