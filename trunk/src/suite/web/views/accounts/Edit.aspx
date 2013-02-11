<%@ Page Language="C#" Title="DL - Account Checker - Accounts - Edit" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DL.AccountChecker.Domain.Account>" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div class="dl-heading">
        <div class="dl-inline-block">
            <h1>Accounts - Edit</h1>
        </div>
    </div>

    <%: Html.ValidationSummary("Please correct the errors and try again.") %>
    
    <% using (Html.BeginForm()) { %>

    <fieldset>
        <ol>
            <li><%: Html.LabelFor(m => m.BankName) %> <%: Html.TextBoxFor(m => m.BankName) %> <%: Html.ValidationMessageFor(m => m.BankName, "*") %></li>
            <li><%: Html.LabelFor(m => m.BankCode) %> <%: Html.TextBoxFor(m => m.BankCode) %> <%: Html.ValidationMessageFor(m => m.BankCode, "*") %></li>
            <li><%: Html.LabelFor(m => m.CardNumber) %> <%: Html.TextBoxFor(m => m.CardNumber) %> <%: Html.ValidationMessageFor(m => m.CardNumber, "*") %></li>
            <li><%: Html.LabelFor(m => m.Locale) %> <%: Html.DropDownListFor(m => m.Locale, new SelectList(new String[] { "en-US", "en-GB" }, Model.Locale)) %> <%: Html.ValidationMessageFor(m => m.Locale, "*") %></li>
            <li><label for="delete">Delete</label> <%: Html.CheckBox("delete", (Model.Status == Status.Inactive)) %></li>
            <li><%: Html.Button("bt-submit", "submit", "Submit") %></li>
        </ol>
    </fieldset>

    <% } %>
    
</asp:Content>
