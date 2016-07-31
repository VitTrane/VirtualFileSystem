using ServiceDomainModel;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ServicesImpl
{
    [Serializable]
    public class LocalFileSystem : IVirtualFileSystem
    {
        private Folder _root;

        public Folder Root
        {
            get { return _root; }
        }

        public LocalFileSystem()
        {
            _root = new Folder("Root","Root");
        }
        
        /// <summary>
        /// Создает файл по указанному пути
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="fileSystemNode">Файл</param>
        public void Create(string path, FileSystemNode fileSystemNode)
        {
            string pathWithoutName = GetPathWithoutName(path);
            string name =GetNameFromPath(path);
            Folder folder = CreateFolders(pathWithoutName);
            if (!folder.Exist(name,fileSystemNode.FileType)) 
            {
                fileSystemNode.Path = path;
                fileSystemNode.Name = name;
                folder.AddFile(fileSystemNode);
            }
        }        

        /// <summary>
        /// Копирует файл, если в новом месте нету файла с таким же названием
        /// </summary>
        /// <param name="sourcePath">Путь к файлу, который нужно скопировать</param>
        /// <param name="newPath">Путь директории, куда нужно скопировать файл</param>
        /// <param name="typeFile">Тип файла</param>
        public void CopyTo(string sourcePath, string newPath, TypeFileSystemNode typeFile)
        {
            try
            {
                string pathWithoutName = GetPathWithoutName(sourcePath);
                string nameFile = GetNameFromPath(sourcePath);

                Folder newFolder = CreateFolders(newPath);
                Folder copyFolder = GetFolder(pathWithoutName).DeepClone();
                FileSystemNode copyFileSystemNode = copyFolder.GetFileSystemNode(nameFile,typeFile);

                if ((copyFileSystemNode != null) && !(newFolder.Exist(copyFileSystemNode.Name, typeFile)))
                {
                    copyFolder.Path = newPath;
                    copyFileSystemNode.Path = newPath;
                    newFolder.AddFile(copyFileSystemNode);
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = string.Format("{0},\n {1}, \n Попытка скопировать несуществующий файл {2}",
                    ex.ToString(), ex.StackTrace, sourcePath);
                Trace.WriteLine(message);
            }
        }

        /// <summary>
        /// Перемещает файл, если в новом месте нету файла с таким же названием
        /// </summary>
        /// <param name="sourcePath">Путь к файлу, который нужно переместить</param>
        /// <param name="newPath">Путь директории, куда нужно переместить файл</param>
        /// <param name="typeFile">Тип файла</param>
        public void MoveTo(string sourcePath, string newPath, TypeFileSystemNode typeFile)
        {
            try
            {
                string pathWithoutName = GetPathWithoutName(sourcePath);
                string nameFile = GetNameFromPath(sourcePath);

                Folder folder = GetFolder(pathWithoutName);
                Folder newParent = CreateFolders(newPath);
                FileSystemNode fileSystemNode = folder.GetFileSystemNode(nameFile, typeFile);

                if ((fileSystemNode != null) && !(newParent.Exist(fileSystemNode.Name, typeFile)))
                {
                    fileSystemNode.Parent.RemoveFile(fileSystemNode);
                    fileSystemNode.Path = newPath;
                    newParent.AddFile(fileSystemNode);
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = string.Format("{0},\n {1}, \n Попытка переместить несуществующий файл {2}",
                    ex.ToString(), ex.StackTrace, sourcePath);
                Trace.WriteLine(message);
            }
        }

        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="typeFile">Тип файла</param>
        public void Delete(string path, TypeFileSystemNode typeFile)
        {
            try
            {
                string pathWithoutName = GetPathWithoutName(path);
                string nameFile = GetNameFromPath(path);

                Folder dir = GetFolder(GetPathWithoutName(path));
                FileSystemNode fileSystemNode = dir.GetFileSystemNode(nameFile, typeFile);
                dir.RemoveFile(fileSystemNode);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = string.Format("{0},\n {1}, \n Попытка удалить несуществующий файл {2}",
                    ex.ToString(),ex.StackTrace, path);

                Trace.WriteLine(message);
            }
        }

        /// <summary>
        /// Возвращает список подкаталогов, заданного каталога
        /// </summary>
        /// <param name="path">Путь каталога</param>
        public IEnumerable<Folder> GetFolders(string path)
        {
            List<Folder> folders = new List<Folder>();
            try
            {
                Folder dir = GetFolder(path);
                folders.AddRange(dir.GetSubfolders());
            }
            catch (System.IO.FileNotFoundException ex)
            {
                string message = string.Format("{0},\n {1}, \n Попытка получить подкаталоги несуществующего каталога {2}",
                    ex.ToString(), ex.StackTrace, path);

                Trace.WriteLine(message);
            }

            return folders;
        }

        /// <summary>
        /// Проверяет, сущетсвует ли путь
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public bool ExistPath(string path)
        {
            string[] foldersNames = path.Split('\\');
            Folder currentFolder = Root;
            for (int i = 0; i < foldersNames.Length; i++)
            {
                if (currentFolder.Exist(foldersNames[i],TypeFileSystemNode.Folder))
                {
                    currentFolder = (Folder)currentFolder.GetFileSystemNode(foldersNames[i], TypeFileSystemNode.Folder);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Возвращает каталог по заданному пути
        /// </summary>
        /// <param name="path">Путь каталога</param>
        private Folder GetFolder(string path)
        {
            if (!ExistPath(path))
                throw new System.IO.FileNotFoundException();

            Folder currentFolder = Root;
            string[] foldersNames = path.Split('\\');
            for (int i = 0; i < foldersNames.Length; i++)
            {
                currentFolder = (Folder)currentFolder.GetFileSystemNode(foldersNames[i], TypeFileSystemNode.Folder);
            }

            return currentFolder;
        }

        /// <summary>
        /// Создает все каталоги, которых не существует в пути,
        /// и возвращает последний созданный
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        private Folder CreateFolders(string path)
        {
            Folder currentFolder = Root;
            string[] foldersNames = path.Split('\\');
            for (int i = 0; i < foldersNames.Length; i++)
            {
                if (currentFolder.Exist(foldersNames[i],TypeFileSystemNode.Folder))
                {
                    currentFolder = (Folder)currentFolder.GetFileSystemNode(foldersNames[i], TypeFileSystemNode.Folder);
                }
                else
                {
                    Folder newFolder = new Folder(
                        currentFolder.Path + "\\" + foldersNames[i], foldersNames[i]);

                    currentFolder.AddFile(newFolder);
                    currentFolder = newFolder;
                }
            }

            return currentFolder;
        }

        /// <summary>
        /// Возвращает путь без имени файла
        /// </summary>
        /// <param name="path">Путь файла</param>
        private string GetPathWithoutName(string path)
        {
            string[] pathParts = path.Split('\\');
            string pathWithoutName = pathParts[0];
            for (int i = 1; i < pathParts.Length - 1; i++)
            {
                pathWithoutName += "\\" + pathParts[i];
            }
            return pathWithoutName;
        }

        /// <summary>
        /// Возвращает имя файла из пути
        /// </summary>
        /// <param name="path">Путь файла</param>
        private string GetNameFromPath(string path)
        {
            string[] pathParts = path.Split('\\');
            string name = pathParts[pathParts.Length - 1];
            return name;
        }
    }
}
