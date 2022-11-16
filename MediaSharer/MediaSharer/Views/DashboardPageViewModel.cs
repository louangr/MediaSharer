using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MediaSharer.Core;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace MediaSharer.Views
{
    public class DashboardPageViewModel : CorePageViewModel
    {
        #region Private fields
        
        private RelayCommand pickImagesCommand;
        private RelayCommand shareImageCommand;
        private FileOpenPicker picker;
        private ObservableCollection<BitmapImage> images;
        private bool isSharingImage;

        #endregion Private fields

        public DashboardPageViewModel()
        {
            Initialize();
        }

        #region Properties

        public bool IsSharingImage
        {
            get => isSharingImage;
            set
            {
                SetProperty(ref isSharingImage, value);
            }
        }

        public ObservableCollection<BitmapImage> Images
        {
            get => images;
            set
            {
                SetProperty(ref images, value);
            }
        }

        public RelayCommand PickImagesCommand
            => pickImagesCommand ?? (pickImagesCommand = new RelayCommand(() => PickImages().ConfigureAwait(false)));

        public RelayCommand ShareImageCommand
            => shareImageCommand ?? (shareImageCommand = new RelayCommand(() => ShareImage()));


        #endregion Properties

        #region Override methods

        public override void LoadState()
        {
            WinRT.Interop.InitializeWithWindow.Initialize(picker, App.Current.WindowHandle);
        }

        #endregion Override methods

        #region Private methods

        private void Initialize()
        {
            images = new ObservableCollection<BitmapImage>();
            picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
        }

        private async Task PickImages()
        {
            List<StorageFile> newFiles = (await picker.PickMultipleFilesAsync()).ToList();

            if (newFiles != null && newFiles.Count > 0)
            {
                newFiles.ForEach(async n => {
                    using (IRandomAccessStream fileStream = await n.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(fileStream);
                        images.Add(bitmapImage);
                        OnPropertyChanged(nameof(Images));
                    }
                });
            }
        }
        
        private void ShareImage()
        {
        }

        #endregion Private methods
    }
}
