namespace PlainApp.Models
{
    public enum SourceType
    {
        File,
        Folder,
        URL,
    }
    public abstract class AExternalSource
    {
        public SourceType Type { get; private set; }
    }

    public class FileModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public long ByteSize { get; set; } = 0;
        public string Extension { get; set; } = string.Empty;
    }

    public class FolderModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public long ByteSize { get; set; } = 0;
        public List<AExternalSource> Children { get; set; } = new List<AExternalSource>();
    }

    public class URLModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
    }
}
