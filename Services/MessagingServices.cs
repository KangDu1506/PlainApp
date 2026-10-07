using PlainApp.Models;

namespace PlainApp.Services
{
    public class MessagingServices
    {
        public const int MaxAttachmentCount = 5;
        public const long MaxAttachmentSizeMB = 200;

        public static bool ValidateMessage(MessageModel message)
        {
            if (string.IsNullOrWhiteSpace(message.Message))
                return false;

            if (message.ExternalSource != null && message.ExternalSource.Count > MaxAttachmentCount)
                return false;

            long totalSize = 0;

            foreach (var attachment in message.ExternalSource ?? new List<AExternalSource>())
            {
                totalSize += (long)(attachment.GetType().GetProperty("ByteSize")?.GetValue(attachment) ?? 0);
            }

            if (totalSize > MaxAttachmentSizeMB * 1024L * 1024L)
            {
                return false;
            }

            return true;
        }

        public static void RenderMessage(MessageModel message)
        {

        }
    }
}
