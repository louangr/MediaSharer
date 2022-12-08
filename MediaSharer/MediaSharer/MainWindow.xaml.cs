using System;
using MediaSharer.Core;
using MediaSharer.Strings;
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
            LoadIcon("icon.ico");
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
            var projectionWindow = new ProjectionWindow();
            projectionWindow.Title = LocalizedStrings.GetString("AppName");

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

        private void LoadIcon(string iconName)
        {
            var hwnd = this.As<IWindowNative>().WindowHandle;
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName, PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);
            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }

        #endregion Private methods
    }
} 
