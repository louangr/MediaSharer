using System.Runtime.Serialization;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Core;

namespace MediaSharer.Models
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "hasThumbnail")]
        public bool HasThumbnail { get; set; }

        [DataMember(Name = "thumbnail")]
        public BitmapImage Thumbnail { get; set; }

        [DataMember(Name = "isImage")]
        public bool IsImage{ get; set; }

        [DataMember(Name = "imageContent")]
        public BitmapImage ImageContent { get; set; }

        [DataMember(Name = "videoContent")]
        public MediaSource VideoContent { get; set; }
    }

}
