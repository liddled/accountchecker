namespace DL.AccountChecker.Suite.WPF
{
    public partial class TransactionEditView
    {
        public readonly static string ViewName = "TransactionEditView";

        public TransactionEditView(TransactionEditViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public TransactionEditViewModel ViewModel
        {
            get { return DataContext as TransactionEditViewModel; }
            private set { DataContext = value; }
        }
    }
}
