namespace DL.AccountChecker.Suite.WPF
{
    public partial class TransactionHistoryView
    {
        public readonly static string ViewName = "TransactionHistoryView";

        public TransactionHistoryView(TransactionHistoryViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public TransactionHistoryViewModel ViewModel
        {
            get { return DataContext as TransactionHistoryViewModel; }
            private set { DataContext = value; }
        }
    }
}
