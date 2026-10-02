namespace PlainApp.Models
{
    public class NodeModel
    {
        public TaskModel TaskData { get; set; } = new TaskModel();
        public int Step { get; set; }
        public float positionX { get; set; }
        public float positionY { get; set; }
        public float sizeX { get; set; }
        public float sizeY { get; set; }
    }
}
