/// SUMMARY: NodeModel
/// Storing information about a node in the application, including its step, title, description, completion status, due date, progress, assigned users, and external sources.
/// *   External sources supports files, links, and other types of external resources that may be associated with the node. Check for more details in the AExternalSource class and its derived classes (FileSource, LinkSource, etc.).

namespace PlainApp.Models
{
    class NodeModel
    {
        // Displayed Data:
        public int Step { get; set; }
        public string Title { get; set; } = "Default Title";
        public string Description { get; set; } = "Let's do something important!";
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
        public float Progress { get; set; } = 0.0f;
        public List<UserModel> AssignedUsers { get; set; } = new List<UserModel>();
        public List<AExternalSource> ExternalSources { get; set; } = new List<AExternalSource>();

        // Data Behind:
        public (float, float) RelativePosition { get; set; } = (0.0f, 0.0f);
        public (float, float) RelativeSize { get; set; } = (0.0f, 0.0f);
    }
}
