namespace PlainApp.Models
{
    public class ChatModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public bool IsGroupChat => Users.Count > 2; // To allow tagging and group chat features
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public string ChatName { get; set; } = string.Empty;
    }
}
