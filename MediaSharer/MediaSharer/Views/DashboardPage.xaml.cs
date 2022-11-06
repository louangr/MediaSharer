using MediaSharer.Core;

namespace MediaSharer.Views
{
    public sealed partial class DashboardPage : CorePage
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(DashboardPageViewModel));
        }
    }
}
