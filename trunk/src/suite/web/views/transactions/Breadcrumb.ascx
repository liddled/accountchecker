<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="breadcrumb">
    <%
        Response.Write(Html.ActionLink("Transactions", "Index", "Transactions"));
        
        if (Html.IsCurrentAction("History", "Transactions"))
        {
            var model = (TransactionHistoryModel)Model;
            Response.Write(" &raquo; " + Html.ActionLink(Html.Encode(model.Account.BankName), "History", "Transactions", new { accountId = model.Account.AccountId }, null));
        }
        
        if (Html.IsCurrentAction("Add", "Transactions"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("Add", "Add", "Transactions"));
        }
        
        if (Html.IsCurrentAction("Edit", "Transactions"))
        {
            Response.Write(" &raquo; " + Html.ActionLink("Edit", "Edit", "Transactions", new { transactionId = ((DL.AccountChecker.Domain.Transaction)Model).TransactionId}, null));
        }
    %>
</div>