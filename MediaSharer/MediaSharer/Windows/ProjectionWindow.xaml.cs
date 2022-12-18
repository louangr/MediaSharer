using Microsoft.UI;
using System;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using AppWindow = Microsoft.UI.Windowing.AppWindow;
using WinRT.Interop;
using CommunityToolkit.Mvvm.Messaging;
using MediaSharer.Messaging;
using Windows.Media.Core;
using Windows.Media.Playback;
using MediaSharer.Models;
using Microsoft.UI.Xaml.Input;
using WinUIEx;
using MediaSharer.Core;
using WinRT;
using MediaSharer.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MediaSharer.Windows
{
    public sealed partial class ProjectionWindow : Window
    {
        private ISettingsRepository settingsRepository;
        private AppWindow m_appWindow;

        public ProjectionWindow()
        {
            InitializeComponent();

            settingsRepository = App.Current.Services.GetService<ISettingsRepository>();
            m_appWindow = GetAppWindowForCurrentWindow();
            InitializeTitleBar();

            WeakReferenceMessenger.Default.Register<StartItemSharingMessage>(this, StartItemSharingMessageReceived);
            WeakReferenceMessenger.Default.Register<StopItemSharingMessage>(this, StopItemSharingMessageReceived);
        }

        #region Public methods

        public void SetPosition()
        {
            var hwnd = this.As<IWindowNative>().WindowHandle;
            var xPosition = DisplayArea.Primary.WorkArea.Width;
            PInvoke.User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_TOP, xPosition, 0, 0, 0, PInvoke.User32.SetWindowPosFlags.SWP_NOSIZE);
        }

        #endregion Public methods

        #region Private methods

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);

            return AppWindow.GetFromWindowId(myWndId);
        }

        private void InitializeTitleBar()
        {
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(titleBar);

            var res = Application.Current.Resources;
            res["WindowCaptionBackground"] = Colors.Transparent;
            res["WindowCaptionBackgroundDisabled"] = Colors.Transparent;
            res["WindowCaptionForeground"] = Colors.Transparent;
            res["WindowCaptionForegroundDisabled"] = Colors.Transparent;
        }

        private void GridContainerDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (settingsRepository.IsProjectionWindowFullScreenEnabled)
            {
                m_appWindow.SetPresenter(m_appWindow.Presenter.Kind != AppWindowPresenterKind.FullScreen ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
            }
        }

        private void StartItemSharingMessageReceived(object recipient, StartItemSharingMessage message)
        {
            if (settingsRepository.IsProjectionWindowAlwaysOnTopWhenSharing) this.SetIsAlwaysOnTop(true);

            if (message.Item.ContentType == ContentType.Image)
            {
                imageElement.Visibility = Visibility.Visible;
                imageElement.Source = message.Item.ImageContent;
            }
            else
            {
                var mediaPlayer = new MediaPlayer() { Source = MediaSource.CreateFromStorageFile(message.Item.File), TimelineController = message.MediaTimelineController, IsMuted = true };
                mediaPlayer.CommandManager.IsEnabled = false;

                mediaPlayerElement.Visibility = Visibility.Visible;
                mediaPlayerElement.SetMediaPlayer(mediaPlayer);
            }

            WeakReferenceMessenger.Default.Send(new StartItemSharingAcknowledgmentReceiptMessage(message.Item.ContentType));
        }

        private void StopItemSharingMessageReceived(object recipient, StopItemSharingMessage message)
        {
            if (settingsRepository.IsProjectionWindowAlwaysOnTopWhenSharing) this.SetIsAlwaysOnTop(false);

            imageElement.Visibility = Visibility.Collapsed;
            mediaPlayerElement.Visibility = Visibility.Collapsed;
            mediaPlayerElement?.MediaPlayer?.Dispose();
        }

        #endregion Private methods
    }
}
