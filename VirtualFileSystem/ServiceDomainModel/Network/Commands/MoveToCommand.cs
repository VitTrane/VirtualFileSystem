using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public class MoveToCommand : Command
    {
        public string SourcePath { get; private set; }

        public string NewPath { get; private set; }

        public TypeFileSystemNode TypeFile { get; private set; }

        public MoveToCommand(string sourcePath, string newPath, TypeFileSystemNode typeFile)
            : base(TypeCommand.MoveTo)
        {
            SourcePath = sourcePath;
            NewPath = newPath;
            TypeFile = typeFile;
        }
    }
}
