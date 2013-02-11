namespace DL.AccountChecker.Suite.WPF
{
    public partial class TransactionAddView
    {
        public readonly static string ViewName = "TransactionAddView";

        public TransactionAddView(TransactionAddViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public TransactionAddViewModel ViewModel
        {
            get { return DataContext as TransactionAddViewModel; }
            private set { DataContext = value; }
        }
    }
}
