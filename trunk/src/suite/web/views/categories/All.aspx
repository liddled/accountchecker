<%@ Page Language="C#" Title="DL - Account Checker - Categories - All" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IDictionary<string,System.Collections.Generic.IList<DL.AccountChecker.Domain.Category>>>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>All Categories</h1>
        </div>
    </div>

    <div class="dl-container">
        <div class="text-right">
            <%: Html.StatusDropDownList("All", "Categories") %>
        </div>
    </div>

    <% foreach (var group in Model) { %>

    <h2><%: group.Key %></h2>
    <div class="tags">
        <% foreach (var category in group.Value) { %>
        <%: Html.ActionLink(category.CategoryName, "Edit", new { categoryId = category.CategoryId }, new { @class = "tag" }) %>
        <% } %>
    </div>

    <% } %>

</asp:Content>