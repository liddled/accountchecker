<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="breadcrumb">
    <%
        Response.Write(Html.ActionLink("Reports", "Index", "Reports"));
        
        if (Html.IsCurrentAction("Details", "Reports"))
        {
            var model = (ReportDetailViewModel)Model;
            Response.Write(" &raquo; " + Html.ActionLink(Html.Encode(model.Account.BankName), "Details", "Reports", new { accountId = model.Account.AccountId }, null));
        }
    %>
</div>