namespace PlainApp.Models
{
    class TaskOnCalendarModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float Progress { get; set; } = 0.0f;
        public bool IsCompleted { get; set; } = false;
    }
}
