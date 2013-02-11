<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IList<DL.AccountChecker.Domain.Category>>" %>

<%: Html.TextBox("", Model.ConvertCategoryListToString(Constants.Space), new { @class = "category-selector" }) %>