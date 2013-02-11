using System;
using System.Collections.Generic;
using System.Reflection;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;
using Microsoft.Office.Interop.Excel;

namespace DL.AccountChecker.Plugins.Excel
{
    public class ActionController
    {
        private readonly PluginForm _form;

        public ExcelAddIn Excel { get; private set; }

        public ActionController(ExcelAddIn excel)
        {
            Excel = excel;
            _form = new PluginForm(this);
            _form.Initialize();
        }

        public void Open()
        {
            _form.Show();
            _form.BringToFront();
            
            /*Range r = ExcellApp.ActiveCell;
            if (r.ListObject != null)
                r = (Range)r.ListObject.Range[0, 1];
            ExtendedProperties xp = GetExtendedProperties(r);
            lmdForm.SetColumnNames(xp, r);*/
        }

        public void Close()
        {
            if (_form != null)
                _form.Hide();
        }

        public ExcelNamespace GetNodes()
        {
            ExcelNamespace nodeList = null;

            ServiceProxyManager.Use<IExcelService>(EndPoints.ExcelService,
                proxy => { nodeList = proxy.GetNodes(); });

            return nodeList;
        }

        public void LoadFromNamespace(string server, string name)
        {
            var properties = GetOrBuildExtendedProperties(server, Excel.Application.ActiveCell, name);
            /*if (!string.IsNullOrEmpty(promptString))
                xp.PromptString = promptString;
            properties.Properties["nt"] = nodeType;*/

            LoadListFromNamespace(properties, Excel.Application.ActiveCell, true);
        }

        private ExtendedProperties GetOrBuildExtendedProperties(string server, Range range, string name)
        {
            var properties = GetExtendedProperties(range);

            if (properties == null)
            {
                return BuildExtendedProperties(server, name);
            }

            return properties;
        }

        public ExtendedProperties GetExtendedProperties(Range range)
        {
            ExtendedProperties properties = null;

            // do we need this method?
            /*if (range != null && range.Comment != null)
            {
                string commentText = range.Comment.Text(Missing.Value, Missing.Value, Missing.Value);
                if (commentText.StartsWith("<ExtendedProperties"))
                {
                    properties = ExtendedProperties.Deserialize(commentText);
                    if (range.Value2 != null)
                        properties.Properties["nm"] = range.Value2.ToString();
                }
            }*/

            return properties;
        }

        private static ExtendedProperties BuildExtendedProperties(string server, string name)
        {
            var properties = new ExtendedProperties { Server = server };
            properties.Properties.Add("ns", name);
            return properties;
        }

        private void LoadListFromNamespace(ExtendedProperties properties, Range loMarker, bool expandAll)
        {
            string name = properties.Properties["fp"];
            if (String.IsNullOrEmpty(name))
                name = properties.Properties["ns"];

            //serverCollection.Ping(xp.Server);
            /*if (properties.Properties["nt"] == ExcelNodeType.DimObject)
            {
                object obj = ((Range)loMarker[1, 2]).Value2;
                if (obj != null && obj is double)
                {
                    try
                    {
                        DateTime dateRequired = new DateTime(1900, 1, 1).AddDays((double)obj - 2);
                        if (!name.EndsWith("]"))
                            name = name + "[]";
                        name = name.Substring(0, name.IndexOf("[")) + string.Format("[{0:dd-MMM-yyyy} 00:00:00]", dateRequired);
                        xp.Properties["fp"] = name;
                        ApplyExtendedPropertiesOnly(xp, loMarker);
                    }
                    catch { }
                }
                else
                {
                    if (!name.EndsWith("]"))
                        name = name + "[]";
                    if (obj == null || obj is double)
                    {
                        name = name.Substring(0, name.IndexOf("["));
                    }
                    else
                        name = name.Substring(0, name.IndexOf("[")) + string.Format("[\"{0}\"]", obj);
                    xp.Properties["fp"] = name;
                    ApplyExtendedPropertiesOnly(xp, loMarker);
                }
            }*/
            /*BasicList bl = LoadBasicListFromProperty(xp);
            if (string.IsNullOrEmpty(bl.Error))
            {
                if (bl.FullPath.EndsWith("]"))
                {
                    xp.Properties["nt"] = ExcelNodeType.DimObject;
                }
                CreateBasicListObject(xp, name, ref loMarker, bl, expandAll, false, false);
            }
            else
                XtraMessageBox.Show(bl.Error);*/
        }

        /*private BasicList LoadListFromProperty(ExtendedProperties properties)
        {
            try
            {
                BasicList bl = serverCollection.GetController(xp.Server).GetMethodTemplate(xp.Properties["ns"]);
                xp.Properties["lt"] = QPRun;
                return bl;
            }
            catch (Exception e)
            {
                return new BasicList { Error = e.Message };
            }
        }*/
    }
}
