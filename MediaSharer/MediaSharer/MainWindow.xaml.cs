using System;
using MediaSharer.Repositories.Interfaces;
using MediaSharer.Strings;
using MediaSharer.Utils;
using MediaSharer.Views;
using MediaSharer.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using WinUIEx;

namespace MediaSharer
{
    public sealed partial class MainWindow : Window
    {
        private readonly string ICON_FILE_NAME = "icon.ico";

        private ProjectionWindow projectionWindow;
        private ISettingsRepository settingsRepository;

        public MainWindow()
        {
            InitializeComponent();

            settingsRepository = App.Current.Services.GetService<ISettingsRepository>();

            WinApi.LoadIcon(this, ICON_FILE_NAME);
            Navigate(typeof(DashboardPage));
            RenderProjectionWindow();

            Closed += (sender, e) => projectionWindow?.Close();
        }

        #region Properties

        public bool IsFullScreen { get; private set; } = false;
        
        #endregion Properties

        #region Public methods

        public void Navigate(Type page, object parameter = null) => ContentFrame.Navigate(page, parameter, new EntranceNavigationTransitionInfo());
        
        public void GoBack() => ContentFrame.GoBack(new EntranceNavigationTransitionInfo());

        public void ToggleFullScreen() {
            IsFullScreen = !IsFullScreen;
            this.SetWindowPresenter(IsFullScreen ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }

        #endregion Public methods

        #region Private methods

        private void RenderProjectionWindow()
        {
            projectionWindow = new ProjectionWindow();
            projectionWindow.Title = LocalizedStrings.GetString("AppName");
            projectionWindow.SetPosition();
            WinApi.LoadIcon(projectionWindow, ICON_FILE_NAME);
            projectionWindow.Maximize();
            if (settingsRepository.IsProjectionWindowFullScreenEnabled) projectionWindow.SetWindowPresenter(AppWindowPresenterKind.FullScreen);
            projectionWindow.Activate();
        }

        #endregion Private methods
    }
} 
