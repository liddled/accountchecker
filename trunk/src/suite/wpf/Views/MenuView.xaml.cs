using System.Windows.Controls;

namespace DL.AccountChecker.Suite.WPF
{
    public partial class MenuView : UserControl
    {
        public MenuView(MenuViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public MenuViewModel ViewModel
        {
            get { return DataContext as MenuViewModel; }
            private set { DataContext = value; }
        }
    }
}
