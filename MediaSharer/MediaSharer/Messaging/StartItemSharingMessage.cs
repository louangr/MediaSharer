using MediaSharer.Models;
using Windows.Media;

namespace MediaSharer.Messaging
{
    public class StartItemSharingMessage
    {
        public readonly Item Item;

        public readonly MediaTimelineController MediaTimelineController;

        public StartItemSharingMessage(Item item, MediaTimelineController mediaTimelineController)
        {
            Item = item;
            MediaTimelineController = mediaTimelineController;
        }
    }
}
