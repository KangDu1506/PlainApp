using System.Text.Json.Serialization;

namespace PlainApp.Models
{
    public enum SourceType
    {
        File,
        Folder,
        URL,
    }

    [JsonDerivedType(typeof(FileModel))]
    [JsonDerivedType(typeof(FolderModel))]
    [JsonDerivedType(typeof(URLModel))]
    public abstract class AExternalSource
    {
        public SourceType Type { get; protected set; }

        public string Name { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
    }

    public class FileModel : AExternalSource
    {
        public long ByteSize { get; set; } = 0;
        public string Extension { get; set; } = string.Empty;

        public FileModel()
        {
            Type = SourceType.File;
        }
    }

    public class FolderModel : AExternalSource
    {
        public long ByteSize { get; set; } = 0;
        public List<AExternalSource> Children { get; set; } = new List<AExternalSource>();

        public FolderModel()
        {
            Type = SourceType.Folder;
        }
    }

    public class URLModel : AExternalSource
    {
        public URLModel()
        {
            Type = SourceType.URL;
        }
    }
}