using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public class CopyToCommand : Command
    {
        public string SourcePath { get; private set; }

        public string NewPath { get; private set; }

        public TypeFileSystemNode TypeFile { get; private set; }

        public CopyToCommand(string sourcePath, string newPath, TypeFileSystemNode typeFile)
            : base(TypeCommand.CopyTo)
        {
            SourcePath = sourcePath;
            NewPath = newPath;
            TypeFile = typeFile;
        }
    }
}
