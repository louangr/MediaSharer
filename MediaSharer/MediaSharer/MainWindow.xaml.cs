using System;
using MediaSharer.Core;
using MediaSharer.Views;
using MediaSharer.Windows;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using WinRT;
using WinUIEx;

namespace MediaSharer
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigate(typeof(DashboardPage));
            RenderProjectionWindow();
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
            ProjectionWindow projectionWindow = new();
            SetProjectionWindowPosition(projectionWindow.As<IWindowNative>().WindowHandle);
            projectionWindow.SetWindowPresenter(AppWindowPresenterKind.FullScreen);
            projectionWindow.Activate();

            Closed += (sender, e) => { projectionWindow.Close(); };
        }

        private void SetProjectionWindowPosition(IntPtr hwnd)
        {
            var xPosition = DisplayArea.Primary.WorkArea.Width;
            PInvoke.User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_TOP, xPosition, 0, 0, 0, PInvoke.User32.SetWindowPosFlags.SWP_NOSIZE);
        }

        #endregion Private methods
    }
} 
