<%@ Page Language="C#" Title="DL - Account Checker - Categories" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IDictionary<DL.AccountChecker.Domain.Account, System.Collections.Generic.IList<DL.AccountChecker.Domain.Category>>>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Categories</h1>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.Button("bt-view", "button", Url.Action("All", "Categories"), "View All") %>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("Index", "Categories") %>
        </div>
    </div>

    <% foreach (var view in Model) { %>

    <div class="dl-list <%: TextRepTx.ToText(view.Key.Status, ETextRepType.Web) %>">
        <div class="dl-inline-block">
            <h2><%: Html.ActionLink(view.Key.BankName, "Details", "Categories", new { accountId = view.Key.AccountId }, null) %></h2>
            <div class="tags dl-inline-block">
                <% foreach (var category in view.Value) { %>
                <%: Html.ActionLink(category.CategoryName, "History", "Transactions", new { accountId = view.Key.AccountId, categories = category.CategoryName }, new { @class = "tag" }) %>
                <% } %>
            </div>
        </div>
        <ol class="dl-inline-block float-right">
            <li class="dl-inline-block dl-box">
                <%: view.Value.Count %>
                <div class="small">Categories</div>
            </li>
        </ol>
    </div>

    <% } %>

</asp:Content>