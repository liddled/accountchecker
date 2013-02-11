<%@ Page Language="C#" Title="DL - Account Checker - Categories - Details" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Suite.Web.CategoryDetailViewModel>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Categories (<%: Model.TotalCategories %>)</h1>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("Details", "Categories") %>
        </div>
    </div>
    
    <% foreach (var groupedCategory in Model.GroupedCategories) { %>

    <% var p = ((double)groupedCategory.Value / (double)Model.TotalCategories) * 100; %>
    <div style="clear:both;">
        <div style="float:left;width:150px;">
            <%: Html.ActionLink(groupedCategory.Key.CategoryName, "History", "Transactions", new { accountId = Model.Account.AccountId, categories = groupedCategory.Key.CategoryName }, new { @class = "tag" }) %>
        </div>
        <div class="graph" style="float:left;width:50%;">
            <strong class="bar" style="width:<%: p.ToString("0.##") %>%;"><span><%: Math.Round(p) %>%</span></strong>
        </div>
    </div>

    <% } %>
    
</asp:Content>
