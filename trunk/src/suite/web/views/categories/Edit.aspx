<%@ Page Language="C#" Title="DL - Account Checker - Categories - Edit" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Domain.Category>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Categories - Edit</h1>
        </div>
    </div>

    <%: Html.ValidationSummary("Please correct the errors and try again.") %>
    
    <% using (Html.BeginForm()) { %>

    <fieldset>
        <ol>
            <li><%: Html.LabelFor(m => m.CategoryName) %> <%: Html.TextBoxFor(m => m.CategoryName) %> <%: Html.ValidationMessageFor(m => m.CategoryName, "*") %></li>
            <li><label for="delete">Delete</label> <%: Html.CheckBox("delete", (Model.Status == Status.Inactive)) %></li>
            <li><%: Html.Button("bt-submit", "submit", "Submit") %></li>
        </ol>
    </fieldset>

    <% } %>
    
</asp:Content>
