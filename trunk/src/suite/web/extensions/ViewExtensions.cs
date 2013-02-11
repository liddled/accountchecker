using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    public static class ViewExtensions
	{
		#region Menu

		public static string Menu(this HtmlHelper helper, IDictionary<string, string> actionLinks, string selectedCss)
        {
			var ul = new TagBuilder("ul");

			foreach (var kvp in actionLinks)
			{
				var anchor = new TagBuilder("a");
				anchor.SetInnerText(kvp.Key);
				anchor.MergeAttribute("href", kvp.Value);
				if (helper.IsCurrentController(kvp.Key)) anchor.MergeAttribute("class", selectedCss);

				var li = new TagBuilder("li");
				li.Attributes.Add("class", kvp.Key.ToLower());
				li.InnerHtml = anchor.ToString();

				ul.InnerHtml += li;
			}

            return ul.ToString();
		}

		#endregion

        #region Editor Helpers

        public static MvcHtmlString ConvertCategoryListToString(this IList<Category> list, string separator)
        {
            if (list == null)
                return null;

            return MvcHtmlString.Create(list.Select(c => c.CategoryName).ConvertListToString(separator));
        }

        #endregion

		#region Google Chart Helpers

        public static MvcHtmlString AnnotatedTimeline(this HtmlHelper helper, string div, decimal lowestBalance, decimal highestBalance, string locale, IDictionary<BusDate, Candlestick> values)
		{
            var sb = new StringBuilder();

            sb.Append("google.load('visualization','1',{packages:['annotatedtimeline']});");
            sb.Append("google.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart(){");
            sb.Append("var data = new google.visualization.DataTable();");
            sb.Append("data.addColumn('date','Date');");
            sb.Append("data.addColumn('number','Balance');");
            sb.Append("data.addColumn('string','title1');");
            sb.Append("data.addColumn('string','text1');");
            sb.AppendFormat("data.addRows({0});", values.Count);

            var counter = 0;
            foreach (var v in values)
            {
                sb.AppendFormat("data.setValue({0},0,new Date({1},{2},{3}));", counter, v.Key.Year, v.Key.Month-1, v.Key.Day);
                sb.AppendFormat("data.setValue({0},1,{1});", counter, v.Value.Close);
                
                if (lowestBalance == v.Value.Close)
                {
                    sb.AppendFormat("data.setValue({0},2,'{1}');", counter, "Lowest Balance");
                    sb.AppendFormat("data.setValue({0},3,'{1}');", counter, lowestBalance.CulturedAmount("C", locale));
                }

                if (highestBalance == v.Value.Close)
                {
                    sb.AppendFormat("data.setValue({0},2,'{1}');", counter, "Highest Balance");
                    sb.AppendFormat("data.setValue({0},3,'{1}');", counter, highestBalance.CulturedAmount("C", locale));
                }
                
                counter++;
            }

            sb.Append("var chart = new google.visualization.AnnotatedTimeLine(document.getElementById('chart_div'));");
            sb.Append("chart.draw(data,{displayAnnotations:true,wmode:'transparent',fill:10});");
            sb.Append("}");

			return MvcHtmlString.Create(sb.ToString());
		}

        public static MvcHtmlString CandlestickGraph(this HtmlHelper helper, IDictionary<BusDate, Candlestick> values, int width, int height)
        {
            var img = new TagBuilder("img");

            int length = values.Count + 2; //pad by 2 so points dont start and end at points on x-axis
            var openValues = new int[length];
            var closeValues = new int[length];
            var highValues = new int[length];
            var lowValues = new int[length];

            int index = 0;

            openValues[index] = -1;
            closeValues[index] = -1;
            highValues[index] = -1;
            lowValues[index] = -1;
            
            index++;

            foreach (var item in values)
            {
                openValues[index] = (int)item.Value.Open;
                closeValues[index] = (int)item.Value.Close;
                highValues[index] = (int)item.Value.High;
                lowValues[index] = (int)item.Value.Low;
                index++;
            }

            openValues[index] = -1;
            closeValues[index] = -1;
            highValues[index] = -1;
            lowValues[index] = -1;

            int min = lowValues.Where(v => v > 0).DefaultIfEmpty().Min();
            int max = highValues.DefaultIfEmpty().Min();

            var sb = new StringBuilder("https://chart.googleapis.com/chart?cht=lc");
            sb.AppendFormat("&chs={0}x{1}", width, height);
            sb.Append("&chm=F,0000FF,0,,20");
            sb.Append("&chxt=y");
            sb.AppendFormat("&chxr=0,{0},{1}", min, max);
            sb.Append("&chd=t0:");

            sb.Append(lowValues.ConvertListToString(Constants.Comma, false)).Append("|");
            sb.Append(openValues.ConvertListToString(Constants.Comma, false)).Append("|");
            sb.Append(closeValues.ConvertListToString(Constants.Comma, false)).Append("|");
            sb.Append(highValues.ConvertListToString(Constants.Comma, false));

            img.Attributes.Add("src", sb.ToString());

            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }

		/*private static string ExtendedEncode(IList<int> values, int maxVal)
		{
			string map = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-.";
			int mapLength = map.Length;

			StringBuilder chartData = new StringBuilder();

			for (int i = 0; i < values.Count; i++)
			{
				var scaledVal = Math.Floor(mapLength * mapLength * values[i] / maxVal);

				if (scaledVal > (mapLength * mapLength) - 1)
				{
					chartData.Append("..");
				}
				else if (scaledVal < 0)
				{
					chartData.Append("__");
				}
				else
				{
					var quotient = Math.Floor(scaledVal / mapLength);
					var remainder = scaledVal - mapLength * quotient;
					//chartData += EXTENDED_MAP.charAt(quotient) + EXTENDED_MAP.charAt(remainder);
				}
			}
			
			return chartData.ToString();
		}

		public static MvcHtmlString RollingBalancesGraph(this HtmlHelper helper, IDictionary<BusDate, decimal> balances, int width, int height, string lineColour, object htmlAttributes)
		{
			var img = new TagBuilder("img");

			StringBuilder sb = new StringBuilder("http://chart.apis.google.com/chart?cht=lc");
			sb.Append("&chs=").Append(width).Append("x").Append(height);

			var balanceValues = balances.Values.ToArray();

			//var data = ExtendedEncode(balanceValues);
			//sb.Append("&chd=e:").Append(data);

			var data2 = string.Join(",", balanceValues.Select(x => x.ToString()).ToArray());
			sb.Append("&chd=t:").Append(data2);

			if (balanceValues.Length > 0)
			{
				var yRange = string.Format("{0},{1}", balanceValues.Min(), balanceValues.Max());
				sb.Append("&chds=").Append(yRange);
			}

			if (!string.IsNullOrEmpty(lineColour))
				sb.Append("&chco=").Append(lineColour);

			img.MergeAttribute("src", sb.ToString());
			img.MergeAttributes(new RouteValueDictionary(htmlAttributes));

			return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
		}*/

		#endregion
	}
}
