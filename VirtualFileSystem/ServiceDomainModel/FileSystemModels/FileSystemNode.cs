using System;

namespace ServiceDomainModel
{   
    [Serializable]
    public abstract class FileSystemNode
    {
        public TypeFileSystemNode FileType { get; protected set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public Folder Parent { get; set; }

        public FileSystemNode()
        {
        }

        public FileSystemNode(string path, string name)
        {
            Path = path;
            Name = name;
        }  
    }
}
