<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="header">
    <div class="inner">
        <div class="hlogo">
            <img src="<%: Url.Content("~/content/images/logo.png") %>" alt="logo" class="logo" />
            <span style="color:#029f1b;">account</span> <span style="color:#fff;"><strong>Checker</strong></span>
        </div>
        <div class="menu">
            <%=
                Html.Menu
                (
                    new Dictionary<string,string>()
                    {
                        {"Accounts",Url.Action("Index","Accounts")},
                        {"Transactions",Url.Action("Index","Transactions")},
                        {"Reports",Url.Action("Index","Reports")},
                        {"Categories",Url.Action("Index","Categories")}
                    },
                    "selected"
                )
            %>
        </div>
        <hr style="margin: 10px 0;" />
        <% Html.RenderPartial("Breadcrumb"); %>
    </div>
</div>