using CommunityToolkit.Mvvm.Messaging;
using MediaSharer.Core;
using MediaSharer.Messaging;
using MediaSharer.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Media;
using Windows.Media.Playback;

namespace MediaSharer.Views
{
    public sealed partial class PlayerPage : CorePage
    {
        private Item item;
        private MediaTimelineController mediaTimelineController;

        public PlayerPage()
        {
            this.InitializeComponent();
        }

        #region Overridden methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            item = e.Parameter as Item;
            Initialize();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            mediaPlayerElement?.MediaPlayer?.Dispose();
        }

        #endregion Overridden methods

        #region Private methods

        private void Initialize()
        {
            if (item.ContentType == ContentType.Image)
            {
                imageElement.Visibility = Visibility.Visible;
                imageElement.Source = item.ImageContent;
                playButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                mediaTimelineController = new MediaTimelineController();
                var mediaPlayer = new MediaPlayer() { Source = item.VideoContent, TimelineController = mediaTimelineController };
                mediaPlayer.CommandManager.IsEnabled = false;

                mediaPlayerElement.Visibility = Visibility.Visible;
                mediaPlayerElement.SetMediaPlayer(mediaPlayer);
            }

            WeakReferenceMessenger.Default.Send(new StartItemSharingMessage(item, mediaTimelineController));
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            var icon = (sender as Button).Content as FontIcon;

            if (mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                mediaTimelineController.Pause();
                icon.Glyph = "\xF5B0";
            }
            else
            {
                mediaTimelineController.Resume();
                icon.Glyph = "\xE103";
            }
        }

        private void FullScreenWindowButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleFullScreen();
            var icon = (sender as Button).Content as FontIcon;
            icon.Glyph = icon.Glyph == "\xE1D9" ? "\xE1D8" : "\xE1D9";
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new StopItemSharingMessage());
            GoBack();
         
            if (IsFullScreen)
            {
                ToggleFullScreen();
            }
        }

        private void GridContainerPointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            playerControls.Visibility = Visibility.Collapsed;
        }

        private void GridContainerPointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            playerControls.Visibility = Visibility.Visible;
        }

        private void GridContainerPointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            playerControls.Visibility = playerControls.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

        }
        
        #endregion Private methods
    }
}
