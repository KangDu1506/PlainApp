namespace PlainApp.Models
{
    public class PlanModel
    {
        public static readonly List<string> StatusStrings = new List<string> { "Not Started", "Just Started","In Progress", "Deadline Coming", "Completed Early", "Completed On Time", "Completed Late" };

        public string UID { get; set; } = Guid.NewGuid().ToString();
        public UserModel Creator { get; set; } = new ();
        public string Title { get; set; } = "The Awesome Plan";
        public string Note { get; set; } = "This is a sample plan note.";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(21);
        public string Status { get; set; } = StatusStrings[0];
        public float Progress { get; set; } = 0.0f;
        public int Priority { get; set; } = 1;
        public List<UserModel> Collaborators { get; private set; } = new();
        public List<NodeModel> Nodes { get; private set; } = new();
        public List<AExternalSource> ExternalSources { get; private set; } = new();
    }
}
