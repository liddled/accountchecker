<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="breadcrumb">
    <%
        Response.Write(Html.ActionLink("Accounts", "Index", "Accounts"));
        
        if (Html.IsCurrentAction("Add", "Accounts"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("Add", "Add", "Accounts"));
        }
        
        if (Html.IsCurrentAction("Edit", "Accounts"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("Edit", "Edit", "Accounts", new { accountId = ((Account)Model).AccountId }, null));
        }
    %>
</div>