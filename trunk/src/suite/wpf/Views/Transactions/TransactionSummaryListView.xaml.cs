namespace DL.AccountChecker.Suite.WPF
{
    public partial class TransactionSummaryListView
    {
        public readonly static string ViewName = "TransactionSummaryListView";

        public TransactionSummaryListView(TransactionSummaryListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public TransactionSummaryListViewModel ViewModel
        {
            get { return DataContext as TransactionSummaryListViewModel; }
            private set { DataContext = value; }
        }
    }
}
