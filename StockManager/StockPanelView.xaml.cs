
namespace StockManager
{
    /// <summary>
    /// Interaction logic for StockPanel.xaml
    /// </summary>
    public partial class StockPanelView
    {
        public StockPanelView()
        {
            InitializeComponent();
            DataContext = new StockPanelViewModel();
        }
    }
}
