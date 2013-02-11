namespace DL.AccountChecker.Suite.WPF
{
    public partial class AccountSummaryListView
    {
        public readonly static string ViewName = "AccountSummaryListView";

        public AccountSummaryListView(AccountSummaryListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public AccountSummaryListViewModel ViewModel
        {
            get { return DataContext as AccountSummaryListViewModel; }
            private set { DataContext = value; }
        }
    }
}
