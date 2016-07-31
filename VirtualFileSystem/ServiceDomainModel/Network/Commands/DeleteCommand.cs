using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public class DeleteCommand : Command
    {
        public string Path { get; private set; }

        public TypeFileSystemNode TypeFile { get; private set; }

        public DeleteCommand(string path, TypeFileSystemNode typeFile)
            : base(TypeCommand.Delete)
        {
            Path = path;
            TypeFile = typeFile;
        }
    }
}
