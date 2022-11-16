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

        #region Public virtual methods

        public virtual void LoadState()
        {

        }

        public virtual void SaveState()
        {
        }

        #endregion
    }
}
