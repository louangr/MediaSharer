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

namespace MediaSharer.Windows
{
    public sealed partial class ProjectionWindow : Window
    {
        private AppWindow m_appWindow;

        public ProjectionWindow()
        {
            this.InitializeComponent();
            m_appWindow = GetAppWindowForCurrentWindow();

            WeakReferenceMessenger.Default.Register<StartItemSharingMessage>(this, StartItemSharingMessageReceived);
            WeakReferenceMessenger.Default.Register<StopItemSharingMessage>(this, StopItemSharingMessageReceived);
        }

        #region Private methods

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);

            return AppWindow.GetFromWindowId(myWndId);
        }

        private void GridContainerDoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            m_appWindow.SetPresenter(m_appWindow.Presenter.Kind != AppWindowPresenterKind.FullScreen ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }

        private void StartItemSharingMessageReceived(object recipient, StartItemSharingMessage message)
        {
            if (message.Item.ContentType == ContentType.Image)
            {
                imageElement.Visibility = Visibility.Visible;
                imageElement.Source = message.Item.ImageContent;
            }
            else
            {
                var mediaPlayer = new MediaPlayer() { Source = MediaSource.CreateFromStorageFile(message.Item.File), TimelineController = message.MediaTimelineController };
                mediaPlayer.CommandManager.IsEnabled = false;

                mediaPlayerElement.Visibility = Visibility.Visible;
                mediaPlayerElement.SetMediaPlayer(mediaPlayer);
            }
        }

        private void StopItemSharingMessageReceived(object recipient, StopItemSharingMessage message)
        {
            imageElement.Visibility = Visibility.Collapsed;
            mediaPlayerElement.Visibility = Visibility.Collapsed;
            mediaPlayerElement?.MediaPlayer?.Dispose();
        }

        #endregion Private methods
    }
}
