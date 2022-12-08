using MediaSharer.Models;

namespace MediaSharer.Messaging
{
    public class StartItemSharingAcknowledgmentReceiptMessage
    {
        public readonly ContentType ContentType;

        public StartItemSharingAcknowledgmentReceiptMessage(ContentType contentType)
        {
            ContentType = contentType;
        }
    }
}
