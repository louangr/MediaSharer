using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MediaSharer.Core
{
    public class CorePageViewModel : ObservableRecipient
    {
        #region Fields

        private bool isLoading;

        #endregion

        #region Properties

        public virtual bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        #endregion

        #region Public methods

        public void Navigate(Type page, object parameter = null) => (App.Current.MainWindow as MainWindow).Navigate(page, parameter);

        public void GoBack() => (App.Current.MainWindow as MainWindow).GoBack();

        public virtual void LoadState()
        {
        }

        public virtual void SaveState()
        {
        }

        #endregion
    }
}
