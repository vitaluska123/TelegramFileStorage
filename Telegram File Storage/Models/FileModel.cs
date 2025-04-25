using System.Collections.Generic;

namespace Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string VirtualPath { get; set; }
        public long Size { get; set; }
        public string Hash { get; set; }
        public bool IsDeleted { get; set; }
        public int PartsCount { get; set; }
        public Dictionary<int, string> FileIds { get; set; }
    }
}
