<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="AaramEducation.Web.Shared.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container-xl py-5 text-center">
    <h1 class="t-h2 mb-3">Something went wrong</h1>
    <p class="t-body mb-4">An unexpected error occurred. Please try again.</p>
    <a href="~/Default.aspx" runat="server" class="btn btn-aaram-primary">Go home</a>
  </div>
</asp:Content>
