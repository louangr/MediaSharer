using MediaSharer.Core;

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
    }
}
