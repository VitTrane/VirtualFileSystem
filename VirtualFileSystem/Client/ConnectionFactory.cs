using Services;

namespace Client
{
    public class ConnectionFactory
    {
        private string _ipAddress;
        private int _port;        

        public ConnectionFactory(string ipAddress,int port)
        {            
            _ipAddress = ipAddress;
            _port = port;
        }

        public IVirtualFileSystem GetFileSystemProxy()
        {
            return new VirtualFileSystemProxy(_ipAddress,_port);
        }
    }
}
