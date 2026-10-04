using PlainApp.Enums;

namespace PlainApp.Models
{
    public class PlanModel
    {
        public string UID { get; set; } = Guid.NewGuid().ToString();
        public UserModel Creator { get; set; } = new ();
        public string Title { get; set; } = "";
        public string Note { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(21);
        public PlanStatusCode Status { get; set; } = PlanStatusCode.NotStarted;
        public SimplifiedPlanStatusCode SimplifiedPlanStatusCode { get; set; } = SimplifiedPlanStatusCode.NotStarted;
        public float Progress { get; set; } = 0.0f;
        public int Priority { get; set; } = 1;
        public List<UserModel> Collaborators { get; private set; } = new();
        public List<NodeModel> Nodes { get; private set; } = new();
        public List<AExternalSource> ExternalSources { get; private set; } = new();
    }
}
