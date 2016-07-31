using ServiceDomainModel;
using System.Collections.Generic;

namespace Services
{
    public interface IVirtualFileSystem
    {
        void Create(string path, FileSystemNode fileSystemNode);
        void CopyTo(string sourcePath, string newPath, TypeFileSystemNode typeFile);
        void MoveTo(string sourcePath, string newPath, TypeFileSystemNode typeFile);
        void Delete(string path, TypeFileSystemNode typeFile);
        IEnumerable<Folder> GetFolders(string path);
    }
}
