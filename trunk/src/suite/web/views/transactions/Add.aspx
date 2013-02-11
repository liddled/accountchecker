<%@ Page Language="C#" Title="DL - Account Checker - Transactions - Add" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Domain.Transaction>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Transactions - Add</h1>
        </div>
    </div>

    <%: Html.ValidationSummary("Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>

    <fieldset>
        <ol>
            <li><%: Html.LabelFor(m => m.AccountId) %> <%: Html.DropDownListFor(m => m.AccountId, new SelectList((IList<Account>)ViewBag.Accounts, "AccountId", "BankName")) %> <%: Html.ValidationMessageFor(m => m.AccountId, "*") %></li>
            <li><%: Html.LabelFor(m => m.UniqueId) %> <%: Html.TextBoxFor(m => m.UniqueId) %> <%: Html.ValidationMessageFor(m => m.UniqueId, "*") %></li>
            <li><%: Html.LabelFor(m => m.Date) %> <%: Html.EditorFor(m => m.Date) %> <%: Html.ValidationMessageFor(m => m.Date, "*") %></li>
            <li><%: Html.LabelFor(m => m.Description) %> <%: Html.TextAreaFor(m => m.Description, new { cols = 55, rows = 5 }) %> <%: Html.ValidationMessageFor(m => m.Description, "*") %></li>
            <li><%: Html.LabelFor(m => m.Amount) %> <%: Html.TextBoxFor(m => m.Amount) %> <%: Html.ValidationMessageFor(m => m.Amount, "*") %></li>
            <li><%: Html.LabelFor(m => m.Categories) %> <%: Html.EditorFor(m => m.Categories) %>  <%: Html.ValidationMessageFor(m => m.Categories, "*") %></li>
            <li><%: Html.Button("bt-submit", "submit", "Submit") %></li>
        </ol>
    </fieldset>

    <% } %>

</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">new dl.accountchecker.CategorySelector('.category-selector');</script>
</asp:Content>