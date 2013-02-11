namespace DL.AccountChecker.Suite.WPF
{
    public partial class CategorySummaryListView
    {
        public readonly static string ViewName = "CategorySummaryListView";

        public CategorySummaryListView(CategorySummaryListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public CategorySummaryListViewModel ViewModel
        {
            get { return DataContext as CategorySummaryListViewModel; }
            private set { DataContext = value; }
        }
    }
}
