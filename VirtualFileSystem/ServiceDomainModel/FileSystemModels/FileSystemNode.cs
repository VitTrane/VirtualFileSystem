using System;

namespace ServiceDomainModel
{   
    [Serializable]
    public abstract class FileSystemNode
    {
        private string _path;
        private string _name;
        private Folder _parent;
        private TypeFileSystemNode fileType;

        public TypeFileSystemNode FileType
        {
            get { return fileType; }
            protected set { fileType = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Folder Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public FileSystemNode()
        {
        }

        public FileSystemNode(string path, string name)
        {
            _path = path;
            _name = name;
        }  
    }
}
