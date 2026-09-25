<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="AaramEducation.Web.Shared.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container d-flex flex-column align-items-center justify-content-center" style="min-height:60vh;text-align:center">
  <h1 style="font-size:4rem;color:var(--aaram-primary)">Oops</h1>
  <p class="lead">Something went wrong.</p>
  <a href='<%= ResolveUrl("~/Default.aspx") %>' class="btn btn-primary mt-3">Back to Home</a>
</div>

</asp:Content>
