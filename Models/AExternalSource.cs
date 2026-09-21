namespace PlainApp.Models
{
    public enum SourceType
    {
        File,
        Folder,
        URL,
    }
    abstract class AExternalSource
    {
        public SourceType Type { get; private set; }
    }

    class FileModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public long ByteSize { get; set; } = 0;
        public string Extension { get; set; } = string.Empty;
    }

    class FolderModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public long ByteSize { get; set; } = 0;
        public List<AExternalSource> Children { get; set; } = new List<AExternalSource>();
    }

    class URLModel : AExternalSource
    {
        public string Name { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
    }
}
