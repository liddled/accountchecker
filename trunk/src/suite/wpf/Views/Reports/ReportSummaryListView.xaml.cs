namespace DL.AccountChecker.Suite.WPF
{
    public partial class ReportSummaryListView
    {
        public readonly static string ViewName = "ReportSummaryListView";

        public ReportSummaryListView(ReportSummaryListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public ReportSummaryListViewModel ViewModel
        {
            get { return DataContext as ReportSummaryListViewModel; }
            private set { DataContext = value; }
        }
    }
}
