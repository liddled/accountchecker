<%@ Page Language="C#" Title="DL - Account Checker - Reports - Details" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Suite.Web.ReportDetailViewModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Rolling Balances</h1>
        </div>
    </div>
    
    <div class="dl-container">
        <div class="dl-inline-block">
            <h2><%: Model.FromDate.ToString("MMM, dd yyyy") %> - <%: Model.ToDate.ToString("MMM, dd yyyy") %></h2>
            <ol class="dl-data-block dl-inline-block">
                <li>
                    <span class="dl-inline-block key">Start Balance</span> 
                    <%: Model.StartBalance.CulturedAmount("C", Model.Account.Locale) %>
                </li>
                <li>
                    <span class="dl-inline-block key">End Balance</span> 
                    <%: Model.EndBalance.CulturedAmount("C", Model.Account.Locale) %>
                </li>
            </ol>
            <ol class="dl-data-block dl-inline-block">
                <li>
                    <span class="dl-inline-block key">Change</span> 
                    <span class="<%: Html.ColourClass(Model.Change) %>"><%: Model.Change.CulturedAmount("C", Model.Account.Locale) %></span>
                </li>
                <li>
                    <span class="dl-inline-block key">% Chg.</span> 
                    <span class="<%: Html.ColourClass(Model.ChangePercentage) %>"><%: Model.ChangePercentage.ToString("+0.00%;-0.00%") %></span>
                </li>
            </ol>
        </div>
        <div class="dl-inline-block float-right">
            <%: Html.TextBox("tb-date-from", Model.FromDate.ToString("dd MMMM yyyy")) %> <%: Html.TextBox("tb-date-to", Model.ToDate.ToString("dd MMMM yyyy")) %>
            <form action="" method="get" class="dl-inline-block">
                <input id="from" name="from" type="hidden" value="<%: Model.FromDate.Sort8Date %>" />
                <input id="to" name="to" type="hidden" value="<%: Model.ToDate.Sort8Date %>" />
                <%: Html.Button("bt", "submit", "Go") %>
            </form>
        </div>
    </div>
    
    <p id="chart_div" style="height:300px;"></p>

</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript" src="http://www.google.com/jsapi"></script>
    <script type="text/javascript"><%: Html.AnnotatedTimeline("chart_div", Model.LowestBalance, Model.HighestBalance, Model.Account.Locale, Model.Balances) %></script>
    <script type="text/javascript">new dl.accountchecker.DatePeriodPicker();</script>
</asp:Content>
