using Microsoft.UI;
using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using WinRT.Interop;

namespace MediaSharer.Windows
{
    public sealed partial class ProjectionWindow : Window
    {
        AppWindow m_appWindow;

        public ProjectionWindow()
        {
            this.InitializeComponent();
            m_appWindow = GetAppWindowForCurrentWindow();
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);

            return AppWindow.GetFromWindowId(myWndId);
        }

        private void Grid_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            m_appWindow.SetPresenter(m_appWindow.Presenter.Kind != AppWindowPresenterKind.FullScreen ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }
    }
}
