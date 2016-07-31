using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public class GetFoldersCommand : Command
    {
        public string Path { get; private set; }

        public GetFoldersCommand(string path)
            : base(TypeCommand.GetFolders)
        {
            Path = path;
        }
    }
}
