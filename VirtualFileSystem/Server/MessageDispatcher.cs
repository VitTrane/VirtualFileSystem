using ServiceDomainModel;
using ServiceDomainModel.Commands;
using ServicesImpl;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    public class MessageDispatcher
    {
        private LocalFileSystem _fileSystem;

        private TcpListener _tcpListener;
        private TcpClient _client;

        public MessageDispatcher(LocalFileSystem fileSystem,string ipAddress, int port)
        {
            _fileSystem = fileSystem;
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            _tcpListener = new TcpListener(ipAddr,port);
        }

        /// <summary>
        /// Запускает ожидание входящих запросов на подключение
        /// </summary>
        public void Start() 
        {
            _tcpListener.Start();            
            while (true)
            {
                _client = _tcpListener.AcceptTcpClient();                
                Command command = GetCommandFromClient(_client);
                DefineCommand(command);
            }
        }        

        /// <summary>
        /// Определяет тип команды и выполняет ее
        /// </summary>
        /// <param name="command">Команда, которую нужно выполнить</param>
        public void DefineCommand(Command command) 
        {
            switch (command.ComandType)
            {
                case TypeCommand.Create:
                    CreateCommand createCommand = (CreateCommand)command;
                    _fileSystem.Create(createCommand.Path, createCommand.TypeFile);
                    SendResult(_client,new Result("Вызвался метод " + createCommand.ComandType));                    
                    break;

                case TypeCommand.Delete:
                    DeleteCommand deleteCommand = (DeleteCommand)command;
                    _fileSystem.Delete(deleteCommand.Path,deleteCommand.TypeFile);
                    SendResult(_client, new Result("Вызвался метод " + deleteCommand.ComandType));
                    break;

                case TypeCommand.MoveTo:
                    MoveToCommand moveCommand = (MoveToCommand)command;
                    _fileSystem.MoveTo(moveCommand.SourcePath,moveCommand.NewPath,moveCommand.TypeFile);
                    SendResult(_client, new Result("Вызвался метод " + moveCommand.ComandType));
                    break;

                case TypeCommand.CopyTo:
                    CopyToCommand copyCommand = (CopyToCommand)command;
                    _fileSystem.CopyTo(copyCommand.SourcePath, copyCommand.NewPath, copyCommand.TypeFile);
                    SendResult(_client, new Result("Вызвался метод " + copyCommand.ComandType));
                    break;

                case TypeCommand.GetFolders:
                    GetFoldersCommand getFoldersCommand = (GetFoldersCommand)command;
                    IEnumerable<Folder> folders = _fileSystem.GetFolders(getFoldersCommand.Path);
                    SendResult(_client,
                        new ResultReturnValue("Вызвался метод " + getFoldersCommand.ComandType, folders));
                    break;
            }

            Console.WriteLine("Вызвался метод " + command.ComandType);
        }

        /// <summary>
        /// Отправляет результат на клиенту
        /// </summary>
        /// <param name="client">TCP клиент, которому нужно отправить результат</param>
        /// <param name="result">Результат, который нужно отправить</param>
        private void SendResult(TcpClient client, Result result)
        {
            NetworkStream networkStream = client.GetStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(networkStream, result);
        }

        /// <summary>
        /// Получает команду от клиента
        /// </summary>
        /// <param name="client">TCP клиент, от которого получает команду </param>
        /// <returns></returns>
        private Command GetCommandFromClient(TcpClient client)
        {
            NetworkStream networkStream = client.GetStream();
            var formatter = new BinaryFormatter();
            Command command = (Command)formatter.Deserialize(networkStream);

            return command;
        }

        /// <summary>
        /// Закрывает прослушивание клиентов
        /// </summary>
        public void Stop() 
        {
            _client.Close();
            _tcpListener.Stop();
        }
    }
}
