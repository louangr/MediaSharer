using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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

        #endregion Properties

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