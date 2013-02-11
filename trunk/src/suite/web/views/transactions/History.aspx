<%@ Page Language="C#" Title="DL - Account Checker - Transactions - History" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Model.TransactionHistoryModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Transaction History</h1>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.Button("bt-add", "button", Url.Action("Add", "Transactions", new { accountId = Model.Account.AccountId }), "Add Transaction") %>
        </div>
    </div>

    <div class="dl-container">
        <div class="dl-inline-block">
            <strong>
                Transactions: <span class="large"><%: Model.TransactionCount %></span>, 
                Net: <span class="<%: Html.ColourClass(Model.Net) %> large"><%: Model.Net.CulturedAmount("C", Model.Account.Locale) %></span>, 
                Outcome: <span class="negative large"><%: Model.Outcome.CulturedAmount("C", Model.Account.Locale) %></span>, 
                Income: <span class="positive large"><%: Model.Income.CulturedAmount("C", Model.Account.Locale) %></span>
            </strong>
        </div>
        <% if (Model.DateView != DateView.All) { %>
        <div class="dl-inline-block float-right">
            <span class="large"><strong><%= Html.PageRouteActionLink("«", "History", "Transactions", new { date = BusDate.GetPreviousDate(Model.Date, Model.DateView).Sort8Date }, null) %> <%: Model.Date.ToString(Model.DateView) %> <%= Html.PageRouteActionLink("»", "History", "Transactions", new { date = BusDate.GetNextDate(Model.Date, Model.DateView).Sort8Date }, null) %> </strong></span>
        </div>
        <% } %>
    </div>

    <div class="dl-container">
        <div class="dl-inline-block">
            <%: Html.ActionLink("» View Reports", "Details", "Reports", new { accountId = Model.Account.AccountId, from = BusDate.GetDate(Model.Date, Model.DateView), to = BusDate.GetNextDate(Model.Date, Model.DateView) }, null) %><br />
            <div class="tags">
            <% foreach (var category in Model.Categories) { %>
                <%: Html.PageRouteActionLink(category.CategoryName, "History", "Transactions", null, new { @class = "tag" }, new { categories = category.CategoryName }) %>
            <% } %>
            </div>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.DateViewDropDownList("History", "Transactions") %> 
            <%: Html.StatusDropDownList("History", "Transactions") %>
        </div>
    </div>

    <% if (Model.TransactionCount > 0) { %>

    <table class="table-list" width="100%" cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th class="date">Date</th>
                <th class="categories">Categories</th>
                <th class="description">Description</th>
                <th class="amount">Amount</th>
                <th class="actions"></th>
            </tr>
        </thead>
        <tbody>
        <% foreach (var transaction in Model.Transactions) { %>
            <tr class="<%: TextRepTx.ToText(transaction.Status, ETextRepType.Web) %>">
                <td class="date">
                    <a name="<%: transaction.TransactionId %>"></a>
                    <%: Html.PageRouteActionLink(transaction.Date.ToString("ddd dd"), "History", "Transactions", new { date = transaction.Date.Sort8Date, view = TextRepTx.ToText(DateView.Day, ETextRepType.Web) }, null) %> 
                    <%: Html.PageRouteActionLink(transaction.Date.ToString("MMM"), "History", "Transactions", new { date = transaction.Date.Sort8Date, view = TextRepTx.ToText(DateView.Month, ETextRepType.Web) }, null) %> 
                    <%: Html.PageRouteActionLink(transaction.Date.ToString("yyyy"), "History", "Transactions", new { date = transaction.Date.Sort8Date, view = TextRepTx.ToText(DateView.Year, ETextRepType.Web) }, null) %>
                </td>
                <td class="categories">
                    <div class="tags">
                    <% foreach (var category in transaction.Categories) { %>
                        <%: Html.PageRouteActionLink(category.CategoryName, "History", "Transactions", new { categories = category.CategoryName }, new { @class = "tag" }) %>
                    <% } %>
                    </div>
                </td>
                <td class="description">
                    <%: Html.ActionLink(transaction.Description, "Edit", "Transactions", new { transactionId = transaction.TransactionId }, new { text = transaction.UniqueId }) %>
                </td>
                <td class="amount <%: Html.ColourClass(transaction.Amount) %>">
                    <% if (transaction.Info.Excluded) { %><s><% } %>
                    <%: transaction.Amount.CulturedAmount("C", Model.Account.Locale) %>
                    <% if (transaction.Info.Excluded) { %></s><% } %>
                </td>
                <td class="actions">
                    <% Html.BeginForm("Exclude", "Transactions", new { transactionId = transaction.TransactionId, exclude = (!transaction.Info.Excluded).ToString().ToLower() }); %>
                    <%: Html.IconButton("bt-exclude", "submit", transaction.Info.Excluded ? "icon-include" : "icon-exclude") %>
                    <% Html.EndForm(); %>
                </td>
            </tr>
        <% } %>
        </tbody>
    </table>

    <% } %>

</asp:Content>

