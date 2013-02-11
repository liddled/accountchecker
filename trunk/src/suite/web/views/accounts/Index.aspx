<%@ Page Language="C#" Title="DL - Account Checker - Accounts" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Suite.Web.AccountIndexViewModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Accounts</h1>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.Button("bt-add", "button", Url.Action("Add", "Accounts"), "Add Account") %>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("Index", "Accounts") %>
        </div>
    </div>

    <% foreach (var view in Model.Views) { %>

    <div class="dl-list <%: TextRepTx.ToText(view.Account.Status, ETextRepType.Web) %>">
        <div class="dl-inline-block">
            <h2><%: Html.ActionLink(view.Account.BankName, "Edit", "Accounts", new { accountId = view.Account.AccountId }, null) %></h2>
            Bank Code: <%: view.Account.BankCode %>, Card Number: <%: view.Account.CardNumber %>
        </div>
        <ol class="dl-inline-block float-right">
            <li class="dl-inline-block dl-box-transaction">
                <%: view.TransactionCount %>
                <div class="small">Transactions</div>
            </li>
            <li class="dl-inline-block dl-box">
                <%: view.Balance.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Balance</div>
            </li>
            <li class="dl-inline-block dl-box-negative">
                <%: view.Outcome.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Outcome</div>
            </li>
            <li class="dl-inline-block dl-box-positive">
                <%: view.Income.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Income</div>
            </li>
        </ol>
    </div>

    <% } %>

</asp:Content>