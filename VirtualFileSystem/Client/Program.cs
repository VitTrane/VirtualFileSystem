using ServiceDomainModel;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Client
{
    class Program
    {
        private static bool _isRepeat = true;

        private static IVirtualFileSystem _fileSystemProxy;
        
        static void Main(string[] args)
        {
            NameValueCollection networkSettings =
                (NameValueCollection)ConfigurationManager.GetSection("networkSettings");

            ConnectionFactory factory = new ConnectionFactory(networkSettings["IPAddress"], int.Parse(networkSettings["Port"]));
            _fileSystemProxy = factory.GetFileSystemProxy();
            Console.WriteLine("Команда для справки : help");
            Console.WriteLine();
            while (_isRepeat)
            {
                Console.Write("Введите команду: >");
                string command = Console.ReadLine();

                try
                {
                    SelectAction(command);
                }
                catch (IndexOutOfRangeException)
                {                    
                    Console.WriteLine("Переданны не все аргументы");
                }
            }
        }

        private static void SelectAction(string command)
        {
            string[] arguments = command.ToLower()
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            switch (arguments[0])
            {
                case "help":
                    ShowHelp();
                    break;

                case "mkdir":
                    _fileSystemProxy.Create(arguments[1],new Folder());
                    break;

                case "crfile":
                    _fileSystemProxy.Create(arguments[1], new File());
                    break;

                case "copydir":
                    _fileSystemProxy.CopyTo(arguments[1], arguments[2], TypeFileSystemNode.Folder);
                    break;

                case "copyfile":
                    _fileSystemProxy.CopyTo(arguments[1], arguments[2], TypeFileSystemNode.File);
                    break;

                case "movedir":
                    _fileSystemProxy.MoveTo(arguments[1], arguments[2], TypeFileSystemNode.Folder);
                    break;

                case "movefile":
                    _fileSystemProxy.MoveTo(arguments[1], arguments[2], TypeFileSystemNode.File);
                    break;

                case "deldir":
                    _fileSystemProxy.Delete(arguments[1], TypeFileSystemNode.Folder);
                    break;

                case "delfile":
                    _fileSystemProxy.Delete(arguments[1], TypeFileSystemNode.File);
                    break;

                case "getdir":
                    List<Folder> folders = _fileSystemProxy.GetFolders(arguments[1])
                        .ToList();
                    OutputDerictories(folders);
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "exit":
                    _isRepeat = false;
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Команда для создания каталога: mkDir <путь>");
            Console.WriteLine("Команда для создания файла: crFile <путь>");
            Console.WriteLine("Команда для копирования каталога: copyDir <путь файла> <путь для копирования>");
            Console.WriteLine("Команда для копирования файла: copyFile <путь файла> <путь для копирования>");
            Console.WriteLine("Команда для перемещения каталога: moveDir <путь> <путь для перемещения>");
            Console.WriteLine("Команда для перемещения файла: moveFile <путь файла> <путь для копирования>");
            Console.WriteLine("Команда для удаления каталога: delDir <путь>");
            Console.WriteLine("Команда для вывода подкаталогов заданного каталога: getDir <путь>");
            Console.WriteLine("Команда для очистки консоли: clear");
            Console.WriteLine("Команда для выхода: exit");
        }

        private static void OutputDerictories(List<Folder> folders)
        {
            foreach (var folder in folders)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("Имя каталога: {0}", folder.Name);
                Console.WriteLine("Кол-во всех файлов: {0}", folder.Files.Count);
                Console.WriteLine("Кол-во подгаталогов: {0}", folder.GetSubfolders().Count);
            }
        }
    }
}
