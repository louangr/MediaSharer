using System;
using MediaSharer.Core;
using MediaSharer.Views;
using MediaSharer.Windows;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT;
using WinUIEx;

namespace MediaSharer
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(typeof(DashboardPage));
            RenderProjectionWindow();
        }

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
    }
} 
