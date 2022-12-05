using System;
using CommunityToolkit.Mvvm.Messaging;
using MediaSharer.Core;
using MediaSharer.Messaging;
using MediaSharer.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using Windows.Media;
using Windows.Media.Playback;

namespace MediaSharer.Views
{
    public sealed partial class PlayerPage : CorePage
    {
        private Item item;
        private MediaTimelineController mediaTimelineController;
        private double videoDurationInSeconds;
        private bool isSeekSliderValueChangeable;

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
                seekSlider.Visibility = Visibility.Collapsed;
            }
            else
            {
                mediaTimelineController = new MediaTimelineController();
                mediaTimelineController.PositionChanged += (o, i) => OnMediaTimelineControllerPositionChanged(o);
                var mediaPlayer = new MediaPlayer() { Source = item.VideoContent, TimelineController = mediaTimelineController };
                mediaPlayer.CommandManager.IsEnabled = false;
                isSeekSliderValueChangeable = true;

                mediaPlayerElement.Visibility = Visibility.Visible;
                mediaPlayerElement.SetMediaPlayer(mediaPlayer);
                mediaPlayerElement.MediaPlayer.PlaybackSession.NaturalDurationChanged += PlaybackSessionNaturalDurationChanged;
                mediaPlayerElement.MediaPlayer.MediaEnded += (o, i) => OnMediaPlayerMediaEnded();
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

        private void Close()
        {
            if (IsFullScreen)
            {
                ToggleFullScreen();
            }

            WeakReferenceMessenger.Default.Send(new StopItemSharingMessage());
            mediaPlayerElement.Visibility = Visibility.Collapsed;
            GoBack();
        }

        private void GridContainerPointerExited(object sender, PointerRoutedEventArgs e)
        {
            playerControls.Visibility = Visibility.Collapsed;
        }

        private void GridContainerPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            playerControls.Visibility = Visibility.Visible;
        }

        private void GridContainerPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            playerControls.Visibility = playerControls.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void PlaybackSessionNaturalDurationChanged(MediaPlaybackSession sender, object args)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                videoDurationInSeconds = sender.NaturalDuration.TotalSeconds;
                seekSlider.Maximum = videoDurationInSeconds;
            });
        }

        private void OnMediaTimelineControllerPositionChanged(MediaTimelineController o)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                if (isSeekSliderValueChangeable)
                {
                    seekSlider.Value = o.Position.TotalSeconds;
                }
            });
        }

        private void SeekSliderManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            isSeekSliderValueChangeable = false;
        }

        private void SeekSliderManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            mediaTimelineController.Position = TimeSpan.FromSeconds((sender as Slider).Value);
            isSeekSliderValueChangeable = true;
        }

        private void SeekSlideValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (isSeekSliderValueChangeable)
            {
                mediaTimelineController.Position = TimeSpan.FromSeconds((sender as Slider).Value);
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e) => Close();

        private void OnMediaPlayerMediaEnded() => DispatcherQueue.TryEnqueue(() => Close());

        #endregion Private methods
    }
}
