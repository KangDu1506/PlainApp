namespace PlainApp.Models
{
    class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Default User";
        public string Email { get; set; } = string.Empty;
    }
}
