using System.Runtime.Serialization;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Core;
using Windows.Storage;

namespace MediaSharer.Models
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "hasThumbnail")]
        public bool HasThumbnail { get; set; }

        [DataMember(Name = "thumbnail")]
        public BitmapImage Thumbnail { get; set; }

        [DataMember(Name = "contentType")]
        public ContentType ContentType { get; set; }

        [DataMember(Name = "imageContent")]
        public BitmapImage ImageContent { get; set; }

        [DataMember(Name = "videoContent")]
        public MediaSource VideoContent { get; set; }

        [DataMember(Name = "file")]
        public StorageFile File { get; set; }
    }

}
