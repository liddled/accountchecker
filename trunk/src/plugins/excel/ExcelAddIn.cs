using System;
using System.Windows.Forms;
using Microsoft.Office.Core;

namespace DL.AccountChecker.Plugins.Excel
{
    public partial class ExcelAddIn
    {
        private ActionController _actionController;

        private CommandBarButton _pluginButton;

        private void ExcelAddInStartup(object sender, EventArgs e)
        {
            _actionController = new ActionController(this);
            AddMenuBar();
        }

        private void ExcelAddInShutdown(object sender, EventArgs e)
        {
            if (_actionController != null)
                _actionController.Close();
            _actionController = null;

            if (_pluginButton != null)
                _pluginButton.Delete();
            _pluginButton = null;
        }

        private void AddMenuBar()
        {
            try
            {
                var activeMenuBar = Application.CommandBars.ActiveMenuBar;

                try
                {
                    _pluginButton = (CommandBarButton)activeMenuBar.Controls["AccountChecker"];
                    _pluginButton.Delete();
                }
                catch { }

                _pluginButton = (CommandBarButton)activeMenuBar.Controls.Add(1, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _pluginButton.Caption = "AccountChecker";
                _pluginButton.Style = MsoButtonStyle.msoButtonCaption;

                _pluginButton.Click += PluginButtonClick;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace, "Addin Failed");
            }
        }

        private void PluginButtonClick(CommandBarButton ctrl, ref bool cancelDefault)
        {
            _actionController.Open();
        }

        private void InternalStartup()
        {
            Startup += ExcelAddInStartup;
            Shutdown += ExcelAddInShutdown;
        }
    }
}
