using CommunityToolkit.Mvvm.Input;
using MediaSharer.Core;
using MediaSharer.Repositories.Interfaces;

namespace MediaSharer.Views
{
    public class SettingsPageViewModel : CorePageViewModel
    {
        #region Private fields
        
        private RelayCommand goBackCommand;
        private ISettingsRepository settingsRepository;

        #endregion Private fields

        public SettingsPageViewModel(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        #region Properties

        public bool AutoPlay
        {
            get => settingsRepository.AutoPlay;
            set
            {
                settingsRepository.AutoPlay = value;
            }
        }

        public RelayCommand GoBackCommand
            => goBackCommand ?? (goBackCommand = new RelayCommand(() => GoBack()));

        #endregion Properties
    }
}
