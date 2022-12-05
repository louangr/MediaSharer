using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MediaSharer.Core;
using MediaSharer.Models;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace MediaSharer.Views
{
    public class DashboardPageViewModel : CorePageViewModel
    {
        #region Private fields

        private readonly List<string> IMAGE_FILE_TYPES = new List<string>() { ".jpg", ".jpeg", ".png" };
        private readonly List<string> VIDEO_FILE_TYPES = new List<string>() { ".mp4", ".avi", ".mkv" };

        private RelayCommand pickFilesCommand;
        private RelayCommand shareItemCommand;
        private FileOpenPicker picker;
        private ObservableCollection<Item> items;
        private Item selectedItem;
        private int itemsCount;
        private RelayCommand navigateToSettingsPageCommand;

        #endregion Private fields

        public DashboardPageViewModel()
        {
            Initialize();
        }

        #region Properties

        public ObservableCollection<Item> Items
        {
            get => items;
            set
            {
                SetProperty(ref items, value);
            }
        }

        public Item SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                ShareItemCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand PickFilesCommand
            => pickFilesCommand ?? (pickFilesCommand = new RelayCommand(() => PickFiles().ConfigureAwait(false)));

        public RelayCommand ShareItemCommand
            => shareItemCommand ?? (shareItemCommand = new RelayCommand(() => Navigate(typeof(PlayerPage), SelectedItem), () => SelectedItem != null));

        public RelayCommand NavigateToSettingsPageCommand
            => navigateToSettingsPageCommand ?? (navigateToSettingsPageCommand = new RelayCommand(() => Navigate(typeof(SettingsPage))));

        #endregion Properties

        #region Public methods

        public void DeleteItem(int id)
        {
            items.Remove(items.SingleOrDefault(i => i.Id == id));
            OnPropertyChanged(nameof(Items));
        }

        #endregion Public methods

        #region Override methods

        public override void LoadState()
        {
            WinRT.Interop.InitializeWithWindow.Initialize(picker, App.Current.WindowHandle);
        }

        #endregion Override methods

        #region Private methods

        private void Initialize()
        {
            itemsCount = 0;
            items = new ObservableCollection<Item>();
            picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            IMAGE_FILE_TYPES.ForEach(t => picker.FileTypeFilter.Add(t));
            VIDEO_FILE_TYPES.ForEach(t => picker.FileTypeFilter.Add(t));
        }

        private async Task PickFiles()
        {
            List<StorageFile> newFiles = (await picker.PickMultipleFilesAsync()).ToList();

            if (newFiles != null && newFiles.Count > 0)
            {
                foreach (var n in newFiles)
                {
                    var newItem = new Item() { Id = itemsCount, File = n };

                    if (IMAGE_FILE_TYPES.Contains(n.FileType))
                    {
                        using (IRandomAccessStream fileStream = await n.OpenAsync(FileAccessMode.Read))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(fileStream);

                            newItem.ImageContent = bitmapImage;
                            newItem.Thumbnail = bitmapImage;
                            newItem.HasThumbnail = true;
                            newItem.ContentType = ContentType.Image;
                        }
                    }
                    else
                    {
                        newItem.VideoContent = MediaSource.CreateFromStorageFile(n);
                        newItem.ContentType = ContentType.Video;
                    }

                    items.Add(newItem);
                    itemsCount++;
                }
                
                OnPropertyChanged(nameof(Items));
            }
        }

        #endregion Private methods
    }
}
