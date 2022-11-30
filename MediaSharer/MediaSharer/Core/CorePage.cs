using Microsoft.UI.Xaml.Controls;

namespace MediaSharer.Core
{
    public class CorePage : Page
    {
        public CorePage()
        {
            Loaded += (o,i) => LoadState();
            Unloaded += (o,i) => SaveState();
        }

        #region Properties

        public virtual CorePageViewModel ViewModel
        {
            get => this.DataContext as CorePageViewModel;
            set => this.DataContext = value;
        }

        public bool IsFullScreen => (App.Current.MainWindow as MainWindow).IsFullScreen;

        #endregion Properties

        #region Public methods

        public void GoBack() => (App.Current.MainWindow as MainWindow).GoBack();

        public void ToggleFullScreen() => (App.Current.MainWindow as MainWindow).ToggleFullScreen();

        #endregion Public methods

        #region Privates methods

        private void LoadState()
        {
            ViewModel?.LoadState();
        }

        private void SaveState()
        {
            ViewModel?.SaveState();
        }

        #endregion Privates methods
    }
}