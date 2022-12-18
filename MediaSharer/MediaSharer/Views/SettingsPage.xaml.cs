using System;
using MediaSharer.Core;
using Microsoft.UI.Xaml.Documents;
using Windows.System;

namespace MediaSharer.Views
{
    public sealed partial class SettingsPage : CorePage
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(SettingsPageViewModel));
        }

        #region Properties

        public SettingsPageViewModel PageViewModel
            => DataContext as SettingsPageViewModel;

        #endregion Properties

        #region Private methods

        private async void ProjectionWindowInfoLabelHyperlinkClick(Hyperlink sender, HyperlinkClickEventArgs args) => await Launcher.LaunchUriAsync(new Uri("ms-settings:taskbar"));
        
        #endregion Private methods
    }
}
