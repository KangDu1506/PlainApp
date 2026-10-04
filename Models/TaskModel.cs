namespace PlainApp.Models
{
    public class TaskModel
    {
        // Displayed Data:
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string? Title { get; set; }
        public string Description { get; set; } = "Let's do something important!";
        public bool IsCompleted { get; set; } = false;
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
        public float Progress { get; set; } = 0.0f;
        public List<UserModel> Assignees { get; set; } = new List<UserModel>();
        public List<AExternalSource> ExternalSources { get; set; } = new List<AExternalSource>();
        public List<AExternalSource> ExternalAttachments { get; set; } = new List<AExternalSource>();
        public List<string> RequiredPreviousTasks { get; set; } = new List<string>();
    }
}
