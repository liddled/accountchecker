namespace DL.AccountChecker.Suite.WPF
{
    public partial class AccountAddView
    {
        public readonly static string ViewName = "AccountAddView";

        public AccountAddView(AccountAddViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public AccountAddViewModel ViewModel
        {
            get { return DataContext as AccountAddViewModel; }
            private set { DataContext = value; }
        }
    }
}
