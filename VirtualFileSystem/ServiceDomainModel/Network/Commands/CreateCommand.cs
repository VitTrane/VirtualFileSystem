using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public class CreateCommand : Command
    {
        public string Path { get; private set; }

        public FileSystemNode TypeFile { get; private set; }

        public CreateCommand( string path, FileSystemNode typeFile)
            : base(TypeCommand.Create)
        {
            Path = path;
            TypeFile = typeFile;
        }
    }
}
