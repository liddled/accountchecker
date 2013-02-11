<%@ Page Language="C#" Title="DL - Account Checker - Transactions" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Suite.Web.TransactionIndexViewModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Overview for <%: BusDate.Today.ToString("MMMM yyyy") %></h1>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.Button("bt-add", "button", Url.Action("Add", "Transactions"), "Add Transaction") %>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("Index", "Transactions") %>
        </div>
    </div>

    <% foreach (var view in Model.Views) { %>

    <div class="dl-list <%: TextRepTx.ToText(view.Account.Status, ETextRepType.Web) %>">
        <div class="dl-inline-block">
            <h2><%: Html.ActionLink(view.Account.BankName, "History", "Transactions", new { accountId = view.Account.AccountId }, null) %></h2>
            Start Balance: <%: view.StartBalance.CulturedAmount("C", view.Account.Locale) %>, Current Balance: <%: view.CurrentBalance.CulturedAmount("C", view.Account.Locale) %>
        </div>
        <ol class="dl-inline-block float-right">
            <li class="dl-inline-block dl-box-transaction">
                <%: view.TransactionCount %>
                <div class="small">Monthly Trans.</div>
            </li>
            <li class="dl-inline-block dl-box">
                <%: view.Net.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Monthly Net</div>
            </li>
            <li class="dl-inline-block dl-box-negative">
                <%: view.Outcome.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Monthly Outcome</div>
            </li>
            <li class="dl-inline-block dl-box-positive">
                <%: view.Income.CulturedAmount("C", view.Account.Locale) %>
                <div class="small">Monthly Income</div>
            </li>
        </ol>
    </div>

    <% } %>

</asp:Content>
