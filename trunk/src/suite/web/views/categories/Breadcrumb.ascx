<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="breadcrumb">
    <%
        Response.Write(Html.ActionLink("Categories", "Index", "Categories"));

        if (Html.IsCurrentAction("All", "Categories"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("All", "All", "Categories"));
        }
        
        if (Html.IsCurrentAction("Details", "Categories"))
        {
            var model = (CategoryDetailViewModel)Model;
            Response.Write(" &raquo; " + Html.ActionLink(Html.Encode(model.Account.BankName), "Details", "Categories", new { accountId = model.Account.AccountId }, null));
        }

        if (Html.IsCurrentAction("Edit", "Categories"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("Edit", "Edit", "Categories", new { categoryId = ((Category)Model).CategoryId }, null));
        }
    %>
</div>