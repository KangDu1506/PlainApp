namespace PlainApp.Models
{
    public enum TaskFlags
    {
        None = 0,
        Important = 1,
        Urgent = 2,
    }
    public class TaskOnCalendarModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float DurationInMinutes => (float)(EndDate - StartDate).TotalMinutes;
        public TaskFlags Flags { get; set; } = TaskFlags.None;
        public float Progress { get; set; } = 0.0f;
        public bool IsCompleted { get; set; } = false;
    }
}
