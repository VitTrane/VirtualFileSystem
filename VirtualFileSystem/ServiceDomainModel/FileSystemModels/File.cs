using System;

namespace ServiceDomainModel
{
    [Serializable]
    public class File : FileSystemNode
    {
        public File()
            : base()
        {
            FileType = TypeFileSystemNode.File;
        }

        public File(string path, string name)
            : base(path, name)
        {
            FileType = TypeFileSystemNode.File;
        }
    }
}
