using MediaSharer.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace MediaSharer.Views
{
    public sealed partial class DashboardPage : CorePage
    {
        public DashboardPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(DashboardPageViewModel));
        }

        #region Properties

        public DashboardPageViewModel PageViewModel
            => DataContext as DashboardPageViewModel;

        #endregion Properties

        #region Private methods

        private void TrashButtonClick(object sender, RoutedEventArgs e) => PageViewModel.DeleteItem((int)(sender as Button).CommandParameter);

        private void ItemDoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => PageViewModel.ShareItemCommand.Execute(null);

        #endregion Private methods
    }
}
