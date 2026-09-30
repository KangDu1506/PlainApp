
using System.Drawing;

/// SUMMARY: NodeModel
/// Storing information about a node in the application, including its step, title, description, completion status, due date, progress, assigned users, and external sources.
/// *   External sources supports files, links, and other types of external resources that may be associated with the node. Check for more details in the AExternalSource class and its derived classes (FileSource, LinkSource, etc.).
namespace PlainApp.Models
{
    public class NodeModel
    {
        // Displayed Data:
        public int Step { get; set; }
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public string? Title { get; set; } = "Default Title";
        public string? Description { get; set; } = "Let's do something important!";
        public bool IsCompleted { get; set; } = false;
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);
        public float Progress { get; set; } = 0.0f;
        public List<UserModel> AssignedUsers { get; set; } = new List<UserModel>();
        public List<AExternalSource> ExternalSources { get; set; } = new List<AExternalSource>();
        public List<string> DependsOnNodeUIDs { get; set; } = new List<string>();

        // Data Behind:
        public Point RelativePosition { get; set; } = new Point(0, 0);
        public Size RelativeSize { get; set; } = new Size(0, 0);
    }
}
