using ServiceDomainModel;
using ServiceDomainModel.Commands;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client
{
    public class VirtualFileSystemProxy : IVirtualFileSystem
    {
        private TcpClient _tcpClient;
        private string _ipAddress;
        private int _port;

        private Result _result;

        public Result Result
        {
            get { return _result; }
            private set { _result = value; }
        }

        public VirtualFileSystemProxy(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;            
        }

        public void Create(string path, FileSystemNode fileSystemNode)
        {            
            SendToServer(new CreateCommand(path,fileSystemNode));            
        }

        public void CopyTo(string sourcePath, string newPath, TypeFileSystemNode typeFile)
        {
            SendToServer(new CopyToCommand(sourcePath, newPath, typeFile));
        }

        public void MoveTo(string sourcePath, string newPath, TypeFileSystemNode typeFile)
        {
            SendToServer(new MoveToCommand(sourcePath, newPath, typeFile));
        }

        public void Delete(string path, TypeFileSystemNode typeFile)
        {
            SendToServer(new DeleteCommand(path,typeFile));
        }

        public IEnumerable<Folder> GetFolders(string path)
        {
            SendToServer(new GetFoldersCommand(path));
            ResultReturnValue result = _result as ResultReturnValue;
            if (result != null)
            {
                IEnumerable<Folder> resultValue = result.Result as IEnumerable<Folder>;

                if (resultValue != null)
                    return resultValue;
                else
                    return new List<Folder>();
            }
            else
                return new List<Folder>();
        }

        public string GetMessageResult() 
        {
            return _result.Message;
        }

        /// <summary>
        /// Получает результат от сервера
        /// </summary>
        /// <param name="client">Подключенный TCP клиент</param>
        private Result GetResultFromServer(TcpClient client) 
        {
            NetworkStream tcpStream = _tcpClient.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            Result result = (Result)formatter.Deserialize(tcpStream);            

            return result;
        }

        /// <summary>
        /// Отправляет команду на сервер
        /// </summary>
        /// <param name="command">Команда, которую нужно отправить</param>
        private void SendToServer(Command command) 
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_ipAddress, _port);
                NetworkStream tcpStream = _tcpClient.GetStream();

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(tcpStream, command);

                _result = GetResultFromServer(_tcpClient);
            }
            catch (Exception ex)
            {
                string message = string.Format("{0},\n {1}, \n Ошибка при передачи сообщения",
                    ex.ToString(), ex.StackTrace);
                Trace.WriteLine(message);
            }
            finally 
            {
                _tcpClient.Close();
            }            
        }
    }
}
