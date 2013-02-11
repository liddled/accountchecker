<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DL.Framework.Common.BusDate>" %>

<%: Html.TextBox("", (Model != null) ? Model.ToString("dd MMMM yyyy") : BusDate.Today.ToString("dd MMMM yyyy"), new { @class = DL.Framework.Web.ViewExtensions.CSS_UI_DATE }) %>