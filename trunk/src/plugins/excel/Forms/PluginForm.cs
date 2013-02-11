using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using DL.Framework.Common;

namespace DL.AccountChecker.Plugins.Excel
{
    public partial class PluginForm : Form
    {
        private readonly ActionController _actionController;

        private readonly string _username;

        public PluginForm(ActionController actionController)
        {
            _actionController = actionController;

            var cn = WindowsIdentity.GetCurrent();
            _username = (cn != null) ? cn.Name : "unknown";

            this.Load += PluginForm_Load;
        }

        public void Initialize()
        {
            InitializeComponent();
            SetTitle();
        }

        private void SetTitle()
        {
            Text = String.Format("Username: {0}", _username);
        }

        private void LoadNodes()
        {
            var excelNamespace = _actionController.GetNodes();
            
            if (excelNamespace == null || excelNamespace.Children == null || excelNamespace.Children.Count == 0)
                return;

            var treeNode = tvNodes.Nodes.Add(excelNamespace.Name);
            AddNodes(treeNode, excelNamespace.Children);
        }

        private void AddNodes(TreeNode parentNode, List<IExcelNode> nodes)
        {
            foreach (var node in nodes)
            {
                var treeNode = parentNode.Nodes.Add(node.Name);
                if (node is ExcelNamespace)
                {
                    var ns = (ExcelNamespace)node;
                    AddNodes(treeNode, ns.Children);
                }
            }
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {
            LoadNodes();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var server = "localhost";
            var ns = tvNodes.SelectedNode.Text.Replace(" ", String.Empty);

            try
            {
                _actionController.Excel.Application.EnableEvents = false;
                _actionController.Excel.Application.ScreenUpdating = false;
                _actionController.LoadFromNamespace(server, ns);
            }
            catch
            {
            }
            finally
            {
                _actionController.Excel.Application.EnableEvents = true;
                _actionController.Excel.Application.ScreenUpdating = true;
            }

            //var client = new Service1Client();
            //var dt = await client.GetDateTimeTaskAsync();

            //ServiceProxyManager.Use<IGuiServer>(EndPoints.GuiServer, server => { dt = server.Ping(); });

            //call async
            //_execelController.Close();
        }
    }
}
