<%@ Page Language="C#" Title="DL - Account Checker - Reports" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Suite.Web.ReportIndexViewModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Overview for <%: BusDate.Today.ToString("MMMM yyyy") %></h1>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("Index", "Reports") %>
        </div>
    </div>

    <% foreach (var view in Model.Views) { %>

    <div class="dl-list <%: TextRepTx.ToText(view.Account.Status, ETextRepType.Web) %>">
        <div class="dl-inline-block">
            <h2><%: Html.ActionLink(view.Account.BankName, "Details", "Reports", new { accountId = view.Account.AccountId }, null) %></h2>
        </div>
        <ol class="dl-inline-block float-right">
            <li class="dl-inline-block dl-box">
                <span class="<%: Html.ColourClass(view.HighestBalance) %>"><%: view.HighestBalance.CulturedAmount("C", view.Account.Locale) %></span>
                <div class="small">Highest Balance</div>
            </li>
            <li class="dl-inline-block dl-box">
                <span class="<%: Html.ColourClass(view.LowestBalance) %>"><%: view.LowestBalance.CulturedAmount("C", view.Account.Locale) %></span>
                <div class="small">Lowest Balance</div>
            </li>
            <li class="dl-inline-block dl-box">
                <span class="<%: Html.ColourClass(view.Change) %>"><%: view.Change.ToString("+0.00;-0.00") %></span>
                <div class="small">Change</div>
            </li>
            <li class="dl-inline-block dl-box">
                <span class="<%: Html.ColourClass(view.ChangePercentage) %>"><%: view.ChangePercentage.ToString("+0.00%;-0.00%") %></span>
                <div class="small">% Chg.</div>
            </li>
        </ol>
    </div>

    <% } %>

</asp:Content>