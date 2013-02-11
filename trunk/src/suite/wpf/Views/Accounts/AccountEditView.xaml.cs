namespace DL.AccountChecker.Suite.WPF
{
    public partial class AccountEditView
    {
        public readonly static string ViewName = "AccountEditView";

        public AccountEditView(AccountEditViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public AccountEditViewModel ViewModel
        {
            get { return DataContext as AccountEditViewModel; }
            private set { DataContext = value; }
        }
    }
}
