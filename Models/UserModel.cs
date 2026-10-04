namespace PlainApp.Models
{
    public enum Relationship
    {
        Self,
        Stranger,
        Collaborator,
    }

    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Default User";
        public string Email { get; set; } = string.Empty;
        public Relationship Relationship { get; set; } = Relationship.Self;
    }
}
