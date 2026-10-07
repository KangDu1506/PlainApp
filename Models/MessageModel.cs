using PlainApp.Enums;

namespace PlainApp.Models
{
    public class MessageModel
    {
        public DateTime Timestamp { get; set; } 
        public string Message { get; set; } = ".";
        public List<AExternalSource>? ExternalSource { get; set; }
        public MessageStatusCode StatusCode { get; set; } = MessageStatusCode.Draft;
        public UserModel Sender { get; set; } = new UserModel();
        public List<UserModel> Recipients { get; set; } = new List<UserModel>();

        public MessageModel(string message, List<AExternalSource>? externalSource = null, MessageStatusCode statusCode = MessageStatusCode.Draft)
        {
            Timestamp = DateTime.Now;
            Message = message;
            ExternalSource = externalSource;
            StatusCode = statusCode;
        }
    }
}
