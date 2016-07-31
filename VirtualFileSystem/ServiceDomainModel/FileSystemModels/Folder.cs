using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServiceDomainModel
{
    [Serializable]
    public class Folder : FileSystemNode
    {
        public List<FileSystemNode> Files { get; private set; }

        public Folder()
            : base()
        {
            Files = new List<FileSystemNode>();
            FileType = TypeFileSystemNode.Folder;
        }

        public Folder(string path, string name)
            : base(path, name)
        {
            Files = new List<FileSystemNode>();
            FileType = TypeFileSystemNode.Folder;
        }

        /// <summary>
        /// Проверяет содержиться ли каталог или файл в директории
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="typeFile">Тип файла</param>
        public bool Exist(string name, TypeFileSystemNode typeFile)
        {
            return Files.Exists(x => (x.FileType == typeFile)
                && (x.Name.Equals(name)));
        }

        public FileSystemNode GetFileSystemNode(string name, TypeFileSystemNode typeFile)
        {
            return Files.FirstOrDefault(x => (x.FileType == typeFile)
                && (x.Name.Equals(name)));
        }

        public List<Folder> GetSubfolders()
        {
            return Files.Where(x => x.FileType == TypeFileSystemNode.Folder)
                .Cast<Folder>()
                .ToList();
        }

        /// <summary>
        /// Добавляет подкаталог или файл
        /// </summary>
        /// <param name="newFile">Имя файла</param>
        public void AddFile(FileSystemNode newFile)
        {
            Files.Add(newFile);
            newFile.Path = Path + "\\" + newFile.Name;
            newFile.Parent = this;
        }

        /// <summary>
        /// Удаляет подкаталог или файл
        /// </summary>
        /// <param name="file">Файл, который нужно удалить</param>
        public void RemoveFile(FileSystemNode file)
        {
            Files.Remove(file);
            file.Parent = null;
        }

        /// <summary>
        /// Производит глубокое копирование объекта
        /// </summary>
        public Folder DeepClone()
        {
            Folder clone = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                    stream.Position = 0;
                    clone = (Folder)formatter.Deserialize(stream);
                    return clone;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
